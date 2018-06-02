using MobileApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebGrease.Css.Extensions;

namespace MobileApi.Helpers
{
    public class TreeRenderer<T> where T : HierarchyNode
    {
        private readonly Func<T, string> locationRenderer;
        private readonly IEnumerable<T> rootLocations;
        private HtmlTextWriter writer;

        public TreeRenderer(
            IEnumerable<T> rootLocations,
            Func<T, string> locationRenderer)
        {
            this.rootLocations = rootLocations;
            this.locationRenderer = locationRenderer;
        }

        public IHtmlString Render()
        {
            writer = new HtmlTextWriter(new StringWriter());
            RenderLocations(rootLocations);
            return MvcHtmlString.Create( writer.InnerWriter.ToString());
        }

        /// <summary>
        /// Recursively walks the location tree outputting it as hierarchical UL/LI elements
        /// </summary>
        /// <param name="locations"></param>
        private void RenderLocations(IEnumerable<T> locations)
        {
            if (locations == null) return;
            if (locations.Count() == 0) return;
            InUl(() => locations.ForEach(location => InLi(() =>
            {
                writer.Write(locationRenderer(location));
                RenderLocations((IEnumerable<T>)location.ChildrenNodes);
            })));
        }

        private void InUl(Action action)
        {
            writer.WriteLine();
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);
            action();
            writer.RenderEndTag();
            writer.WriteLine();
        }

        private void InLi(Action action)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Li);
            action();
            writer.RenderEndTag();
            writer.WriteLine();
        }
    }
}