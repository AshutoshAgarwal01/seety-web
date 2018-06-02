using MobileApi.Models.Category;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileApi.Extensions.Lib;
using MobileApi.Utilities;
using MobileApi.DataAccess;
using System.Threading.Tasks;

namespace MobileApi.Controllers.WebApp
{
    public class CategoryUploadController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Process(HttpPostedFileBase file)
        {
            List<Models.Category.ExcelRow> rows = null;
            if (Path.GetExtension(file.FileName) == ".xlsx")
            {
                ExcelPackage package = new ExcelPackage(file.InputStream);
                rows = package.Rows();
            }
            var categoryNodes = rows.Select(r => new CategoryNode(r)).ToList();
            
            //Delete all CategoryNodes first
            var query = string.Format(@"select * from c where c.DocumentType = ""{0}""", Constants.CategoryDocumentTypeName);
            await DocumentDBRepository.BulkDelete(query);

            //Create 'em all
            await DocumentDBRepository.BulkCreate(categoryNodes);
            return View(rows);
        }
    }
}