using SAP.Web.Infrastructrue.Filters;
using System.Web.Mvc;

namespace SAP.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CheckBrowserFilter()); //filtr akcji globalny do sprawdzania przeglądarki
        }
    }
}