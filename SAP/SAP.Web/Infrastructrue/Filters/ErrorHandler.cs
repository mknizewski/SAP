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
            filterContext.ExceptionHandled = true;

            WriteLog(ex, filterContext.HttpContext.User.Identity.Name);

            var model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");

            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model)
            };
        }

        public static void WriteLog(Exception ex, string user)
        {
            SAPDbContext dbContext = SAPDbContext.Create();

            var excep = new Exceptions
            {
                UserName = user,
                Type = ex.GetType().Name,
                Message = ex.Message,
                Source = ex.Source,
                InnerException = ex.InnerException != null ? ex.InnerException.Message : null,
                StackTrace = ex.StackTrace,
                InsertTime = DateTime.Now
            };

            dbContext.Exceptions.Add(excep);
            dbContext.SaveChanges();
            dbContext.Dispose();
        }
    }
}