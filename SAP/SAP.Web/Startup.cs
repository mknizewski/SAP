using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SAP.Web.Startup))]
namespace SAP.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
