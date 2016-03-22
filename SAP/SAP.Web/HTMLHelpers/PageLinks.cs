using System;
using System.Text;
using System.Web.Mvc;

namespace SAP.Web.HTMLHelpers
{
    /// <summary>
    /// Klasa budująca paginacje stron
    /// </summary>
    public static class PageLinks
    {
        public static MvcHtmlString Page(this HtmlHelper html, int currentPage, int totalPages, Func<int ,string> pageUrl)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 1; i <= totalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");

                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();

                if (i == currentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }

                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}