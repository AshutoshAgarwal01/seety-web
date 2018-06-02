using MobileApi.Enum;
using MobileApi.Models.Category;
using System;
using System.Collections.Generic;

namespace MobileApi.Models
{
    public class HierarchyNode
    {
        public int NodeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public OptionType OptionType { get; set; }
        public List<HierarchyNode> ChildrenNodes { get; set; }

        public HierarchyNode()
        {

        }

        public HierarchyNode(CategoryNode categoryNode)
        {
            this.NodeId = categoryNode.NodeId;
            this.Name = categoryNode.Name;
            this.Description = categoryNode.Description;
            this.OptionType = categoryNode.OptionType;
        }
    }
}