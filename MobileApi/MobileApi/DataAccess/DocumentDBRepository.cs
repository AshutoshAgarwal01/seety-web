namespace MobileApi.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;

    public static class DocumentDBRepository //<T> where T : class
    {
        private static readonly string DatabaseId = ConfigurationManager.AppSettings["database"];
        private static readonly string CollectionId = ConfigurationManager.AppSettings["collection"];
        private static DocumentClient client;

        public static async Task<T> GetItemAsync<T>(string id, string category)
        {
            try
            {
                Document document =
                    await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return default(T);
                }
                else
                {
                    throw;
                }
            }
        }

        public static async Task<IEnumerable<T>> GetItemsAsync<T>(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public static async Task<Document> CreateItemAsync<T>(T item)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);
            return await client.CreateDocumentAsync(uri, item);
        }

        public static async Task<Document> UpdateItemAsync<T>(string id, T item)
        {
            return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), item);
        }

        public static async Task DeleteItemAsync(string id, string category)
        {
            await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
        }

        public static void Initialize()
        {
            client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["endpoint"]), ConfigurationManager.AppSettings["authKey"]);
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        private static async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection
                        {
                            Id = CollectionId
                        },
                        new RequestOptions { OfferThroughput = 400 });
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Create multiple documents at once in Cosmos Db collection using stored procedure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">List of documents (in C# object form)</param>
        /// <param name="log"></param>
        public static async Task BulkCreate<T>(List<T> data)
        {
            var storedProcedureName = "bulkCreate";
            var collectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);
            StoredProcedure storedProcedure = client.CreateStoredProcedureQuery(collectionUri).Where(c => c.Id == storedProcedureName).AsEnumerable().FirstOrDefault();
            try
            {
                var result = await client.ExecuteStoredProcedureAsync<int>(storedProcedure.SelfLink, data);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete all documents from collection.
        /// </summary>
        /// <param name="log"></param>
        /// <returns>Returns indicator for success or failure.</returns>
        public static async Task<int> BulkDelete(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return 0;
            }
            var storedProcedureName = "bulkDelete";
            var collectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);
            StoredProcedure storedProcedure = client.CreateStoredProcedureQuery(collectionUri).Where(c => c.Id == storedProcedureName).AsEnumerable().FirstOrDefault();

            try
            {
                return await client.ExecuteStoredProcedureAsync<int>(storedProcedure.SelfLink, query);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}