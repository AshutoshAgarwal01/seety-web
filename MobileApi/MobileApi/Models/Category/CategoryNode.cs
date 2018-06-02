using MobileApi.Enum;
using System;
using System.Collections.Generic;

namespace MobileApi.Models.Category
{
    public class CategoryNode
    {
        public CategoryNode()
        {

        }

        public CategoryNode(ExcelRow row)
        {
            var isInt = int.TryParse(row.NodeId, out int tempNodeId);
            if (!isInt || tempNodeId == 0)
            {
                throw new Exception("Invalid node id.");
            }

            this.NodeId = tempNodeId;
            this.Name = row.Name;
            this.Description = row.Description;
            this.OptionType = row.OptionType.ToUpper() == "MULTIPLE" ? OptionType.MULTIPLE : OptionType.SINGLE;

            if (!string.IsNullOrEmpty(row.Children))
            {
                var childIds = row.Children.Split(',');
                if ((childIds.Length == 1 && childIds[0].Trim() != "0") || childIds.Length > 1)
                {
                    foreach (var id in childIds)
                    {
                        int.TryParse(id.Trim(), out int tempId);
                        if (tempId == 0)
                        {
                            throw new Exception("Invalid child node id");
                        }
                        if (Children == null)
                        {
                            Children = new List<int>();
                        }
                        Children.Add(tempId);
                    }
                }
            }

            if(!string.IsNullOrEmpty(row.IsRoot) && bool.TryParse(row.IsRoot.Trim(), out bool isRoot))
            {
                this.IsRoot = isRoot;
            }
            else
            {
                throw new Exception("Invalid IsRoot");
            }
        }

        public int NodeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public OptionType OptionType { get; set; }
        public bool IsRoot { get; set; }
        public List<int> Children { get; set; }
        public String DocumentType
        {
            get { return "HierarchyNode";  }
        }
    }
}