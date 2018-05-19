using MobileApi.DataAccess;
using MobileApi.Models;
using MobileApi.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace MobileApi.Controllers
{
    public class OrderController : ApiController
    {
        // GET: api/Order
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Order/5
        public Order Get(int orderId)
        {
            return new Order();
        }

        // POST: api/Order
        public async Task<IHttpActionResult> Post([FromBody]Order order)
        {
            if(order == null)
            {
                return Content(HttpStatusCode.BadRequest, "Invalid order.");
            }

            //order = MockedData.GetOrder();

            //var result = await DocumentDBRepository<Order>.CreateItemAsync(order);
            var result = await DocumentDBRepository.CreateItemAsync<Order>(order);

            try
            {
                Email.SendEMail(order.Customer.Email, "Hi there", "This is a test email");
            }
            catch(Exception e)
            {
                var p = 1;
            }

            return Ok(result);
        }

        // POST: api/Test
        [HttpPost]
        [Route("api/Order/Test")]
        public async Task<IHttpActionResult> Test([FromBody]HierarchyNode order)
        {
            if (order == null)
            {
                return Content(HttpStatusCode.Accepted, "Invalid order.");
            }

            //order = MockedData.GetOrder();

            //var result = await DocumentDBRepository<HierarchyNode>.CreateItemAsync(order);
            var result = await DocumentDBRepository.CreateItemAsync<HierarchyNode>(order);

            return Ok(result);
        }

        // PUT: api/Order/5
        public void Put(int orderId, [FromBody]Order value)
        {
        }

        // DELETE: api/Order/5
        public void Delete(int orderId)
        {
        }
    }
}
