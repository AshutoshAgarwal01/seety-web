using MobileApi.DataAccess;
using MobileApi.Models.Category;
using MobileApi.Utilities;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MobileApi.Controllers.WebApp
{
    public class CategoryController : Controller
    {
        // GET: Category
        public async Task<ActionResult> Index()
        {
            var categoryNodes = await DocumentDBRepository.GetItemsAsync<CategoryNode>(c => c.DocumentType == Constants.CategoryDocumentTypeName);
            var hierarchyNodes = NodeUtility.GetHierarchyNodesFromCategories(categoryNodes.ToList());
            return View(hierarchyNodes);
        }
    }
}
