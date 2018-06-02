using MobileApi.Models;
using MobileApi.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApi.Utilities
{
    public class NodeUtility
    {
        public static List<HierarchyNode> GetHierarchyNodesFromCategories(List<CategoryNode> categoryNodes)
        {
            var hierarchyNodes = new List<HierarchyNode>();

            var rootCategories = categoryNodes.Where(c => c.IsRoot).ToList();

            foreach (var root in rootCategories)
            {
                var node = new HierarchyNode(root);
                List<HierarchyNode> children = GetChildren(root, categoryNodes);
                node.ChildrenNodes = children;
                hierarchyNodes.Add(node);
            }

            return hierarchyNodes;
        }

        private static List<HierarchyNode> GetChildren(CategoryNode root, List<CategoryNode> categoryNodes)
        {
            List<HierarchyNode> children = null;
            if (root != null && root.Children != null && root.Children.Count > 0)
            {
                children = new List<HierarchyNode>();
                foreach (var c in root.Children)
                {
                    var catNode = categoryNodes.FirstOrDefault(n => n.NodeId == c);
                    var hNode = new HierarchyNode(catNode);
                    var hNodeChildren = GetChildren(catNode, categoryNodes);
                    hNode.ChildrenNodes = hNodeChildren;
                    children.Add(hNode);
                }
            }
            return children;
        }
    }
}