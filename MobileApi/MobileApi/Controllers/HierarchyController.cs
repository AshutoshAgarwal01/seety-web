using MobileApi.DataAccess;
using MobileApi.Models.Category;
using MobileApi.Utilities;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace MobileApi.Controllers
{
    public class HierarchyController : ApiController
    {
        //// GET: api/Hierarchy
        public async Task<IHttpActionResult> Get()
        {
            var categoryNodes = await DocumentDBRepository.GetItemsAsync<CategoryNode>(c => c.DocumentType == Constants.CategoryDocumentTypeName);
            if (categoryNodes == null || categoryNodes.Count() == 0)
            {
                return Content(HttpStatusCode.NotFound, "Hierarchy not found.");
            }

            var hierarchyNodes = NodeUtility.GetHierarchyNodesFromCategories(categoryNodes.ToList());
            if (hierarchyNodes == null || hierarchyNodes.Count() == 0)
            {
                return Content(HttpStatusCode.NotFound, "Hierarchy not found.");
            }

            return Ok(hierarchyNodes);
        }

        // GET: api/Hierarchy/5
        public async Task<IHttpActionResult> Get(int Id)
        {
            var categoryNodes = await DocumentDBRepository.GetItemsAsync<CategoryNode>(c => c.DocumentType == Constants.CategoryDocumentTypeName);
            if (categoryNodes == null || categoryNodes.Count() == 0)
            {
                return Content(HttpStatusCode.NotFound, "Category not found.");
            }

            var hierarchyNodes = NodeUtility.GetHierarchyNodesFromCategories(categoryNodes.ToList());
            var hierarchyRoot = hierarchyNodes.FirstOrDefault(h => h.NodeId == Id);
            if (hierarchyRoot == null)
            {
                return Content(HttpStatusCode.NotFound, "Category not found.");
            }

            return Ok(hierarchyRoot);
        }
    }
}
