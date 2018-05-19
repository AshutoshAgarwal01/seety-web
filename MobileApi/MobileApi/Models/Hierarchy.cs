using System.Collections.Generic;

namespace MobileApi.Models
{
    public class Hierarchy : HierarchyNode
    {
        public List<HierarchyNode> Children { get; set; }
    }
}