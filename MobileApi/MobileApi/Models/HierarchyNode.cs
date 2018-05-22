using MobileApi.Enum;
using System;
using System.Collections.Generic;

namespace MobileApi.Models
{
    public class HierarchyNode
    {
        public int NodeId { get; set; }
        //public int ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public OptionType OptionType { get; set; }
        public List<HierarchyNode> ChildrenNodes { get; set; }

        public String DocumentType
        {
            set { value = "HierarchyNode"; }
        }
    }
}