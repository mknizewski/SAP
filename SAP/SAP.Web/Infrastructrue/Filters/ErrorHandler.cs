using SAP.DAL.DbContext.SAP;
using SAP.DAL.Tables;
using System;
using System.Web.Mvc;

namespace SAP.Web.Infrastructrue.Filters
{
    /// <summary>
    /// Handling errorów
    /// </summary>
    public class ErrorHandler : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            SAPDbContext dbContext = SAPDbContext.Create();

            var exception = new Exceptions
            {
                UserId = filterContext.HttpContext.User.Identity.Name,
                Type = ex.GetType().Name,
                Message = ex.Message,
                Source = ex.Source,
                InnerException = ex.InnerException.Message,
                StackTrace = ex.StackTrace,
                InsertTime = DateTime.Now
            };

            dbContext.Exceptions.Add(exception);
            filterContext.ExceptionHandled = true;

            var model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");

            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model)
            };

            dbContext.Dispose();
        }
    }
}