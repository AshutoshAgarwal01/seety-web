using System;

namespace MobileApi.Models.Category
{
    public class ExcelRow
    {
        public string NodeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OptionType { get; set; }
        public string IsRoot { get; set; }
        public string Children { get; set; }
    }
}