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
        public ActionResult Process(HttpPostedFileBase file)
        {
            List<Models.Category.ExcelRow> rows = null;
            if (Path.GetExtension(file.FileName) == ".xlsx")
            {
                ExcelPackage package = new ExcelPackage(file.InputStream);
                rows = package.Rows();
            }
            var categoryNodes = rows.Select(r => new CategoryNode(r)).ToList();
            var hierarchyNodes = NodeUtility.GetHierarchyNodesFromCategories(categoryNodes);
            return View(rows);
        }
    }
}