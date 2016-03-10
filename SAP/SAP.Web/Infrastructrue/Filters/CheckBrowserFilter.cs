using SAP.BOL.HelperClasses;
using System;
using System.Web;
using System.Web.Mvc;

namespace SAP.Web.Infrastructrue.Filters
{
    /// <summary>
    /// Filtr globalny akcji sprawdzający przeglądarkę
    /// </summary>
    public class CheckBrowserFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string browser = filterContext.HttpContext.Request.Browser.Browser;

            if (String.Equals(browser, "InternetExplorer"))
            {
                try
                {
                    HttpCookie cookie = filterContext.HttpContext.Request.Cookies.Get("BrowserAlert");

                    if (!cookie.Value.Equals(Boolean.TrueString))
                    {
                        filterContext.Controller.TempData["Alert"] = SetAlert.Set("Przeglądarka IE oraz Edge nie jest wspierana przez serwis.", "Uwaga", AlertType.Warning);

                        HttpCookie cookieBrowser = new HttpCookie("BrowserAlert", Boolean.TrueString);
                        cookieBrowser.Expires = DateTime.Now.AddDays(7.0d);

                        filterContext.HttpContext.Request.Cookies.Add(cookieBrowser);
                    }
                }
                catch { }
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }
    }
}