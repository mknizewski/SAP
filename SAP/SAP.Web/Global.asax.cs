using SAP.BOL.LogicClasses;
using SAP.DAL.DBContext;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SAP.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbContext.DataBaseInitializer());
            ServerTime.Inicialize();
        }

        protected void Application_End()
        {
            ServerTime.Dispose();
        }
    }
}