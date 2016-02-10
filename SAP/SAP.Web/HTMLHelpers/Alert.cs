using SAP.BOL.HelperClasses;
using System;
using System.Web.Mvc;

namespace SAP.Web.HTMLHelpers
{
    public class Alert
    {
        public static MvcHtmlString GetAlert(object type)
        {
            if (type != null)
            {
                var typeAlert = type as SetAlert;
                var divTag = new TagBuilder("div");

                divTag.AddCssClass("alert " + typeAlert.TypeOfAlert);

                if (typeAlert.StrongMessage != String.Empty)
                {
                    var strongTag = new TagBuilder("strong");

                    strongTag.SetInnerText(typeAlert.StrongMessage);
                    divTag.InnerHtml = strongTag.ToString(TagRenderMode.Normal) + " ";
                }

                divTag.InnerHtml += typeAlert.Message;

                return MvcHtmlString.Create(divTag.ToString(TagRenderMode.Normal));
            }

            return MvcHtmlString.Empty;
        }
    }
}