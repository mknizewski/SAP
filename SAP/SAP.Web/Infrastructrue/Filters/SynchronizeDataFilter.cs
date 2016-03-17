using SAP.BOL.LogicClasses;
using System.Web.Mvc;

namespace SAP.Web.Infrastructrue.Filters
{
    public class SynchronizeDataFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            bool synchro = ServerConfig.SynchronizeData;

            if (synchro)
                filterContext.Result = new ViewResult
                {
                    ViewName = "SyncData"
                };
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }
    }
}