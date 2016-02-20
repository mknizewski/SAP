using System.Web.Mvc;

namespace SAP.Web.Areas.Tournament
{
    public class TournamentAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Tournament";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Tournament_default",
                "Tournament/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}