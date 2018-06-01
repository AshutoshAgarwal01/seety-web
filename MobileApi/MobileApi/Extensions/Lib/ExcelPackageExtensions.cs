using MobileApi.Models.Category;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;

namespace MobileApi.Extensions.Lib
{
    public static class ExcelPackageExtensions
    {
        public static List<CategoryNode> GetNodes(this ExcelPackage package)
        {
            var worksheet = package.Workbook.Worksheets.Where(s => s.Name.ToLower() == "hierarchy").FirstOrDefault();

            if(worksheet == null)
            {
                throw new Exception("Worksheet named Hierarchy not found");
            }

            var sheetObject = GetObjectFromSheet(worksheet);
            var nodesJson = JsonConvert.SerializeObject(sheetObject.Values.First());
            return JsonConvert.DeserializeObject<List<CategoryNode>>(nodesJson);
        }

        public static string ToJson(this ExcelPackage package)
        {
            var excelObject = new List<Dictionary<string, object>>();

            foreach (var workSheet in package.Workbook.Worksheets)
            {
                var sheetObject = GetObjectFromSheet(workSheet);
                excelObject.Add(sheetObject);
            }

            return new JavaScriptSerializer().Serialize(excelObject);
        }

        private static List<string> GetColumnNames(ExcelWorksheet workSheet)
        {
            var columnNames = new List<string>();

            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                columnNames.Add(firstRowCell.Text);
            }

            return columnNames;
        }

        private static Dictionary<string, object> GetObjectFromSheet(ExcelWorksheet workSheet)
        {
            var columnNames = GetColumnNames(workSheet);
            var newRows = new List<Dictionary<string, string>>();

            for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                var col = 0;
                var jsonRow = new Dictionary<string, string>();
                foreach (var cell in row)
                {
                    jsonRow.Add(columnNames[col], cell.Text);
                    col++;
                }
                newRows.Add(jsonRow);
            }
            var sheetObject = new Dictionary<string, object>();
            sheetObject.Add(workSheet.Name, newRows);
            return sheetObject;
        }
    }
}