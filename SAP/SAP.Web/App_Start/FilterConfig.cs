﻿using SAP.Web.Infrastructrue.Filters;
using System.Web.Mvc;

namespace SAP.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorHandler());
            filters.Add(new SynchronizeDataFilter()); //filtr akcji do sprawdzania synchronizacji danych
        }
    }
}