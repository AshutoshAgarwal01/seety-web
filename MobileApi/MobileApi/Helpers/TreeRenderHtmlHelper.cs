using MobileApi.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace MobileApi.Helpers
{
    public static class TreeRenderHtmlHelper
    {
        public static IHtmlString RenderTree<T>(
            this HtmlHelper htmlHelper,
            IEnumerable<T> rootLocations,
            Func<T, string> locationRenderer)
            where T : HierarchyNode
        {
            return new TreeRenderer<T>(rootLocations, locationRenderer).Render();
        }
    }
}