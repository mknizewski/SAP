using Microsoft.AspNet.Identity;
using SAP.DAL.DbContext;
using SAP.DAL.DbContext.SAP;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace SAP.Web.Infrastructrue.Filters
{
    /// <summary>
    /// Filtr sprawdzający czy dany użytkownik znajduje się w danym turnieju
    /// </summary>
    public class TournamentsAuthorization : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            IIdentity ident = filterContext.Principal.Identity;
            SAPDbContext dbContext = SAPDbContext.Create();
            string sTourId = filterContext.Controller.ValueProvider.GetValue("tourId").AttemptedValue;
            int tourId;
            string userId = ident.GetUserId();

            Int32.TryParse(sTourId, out tourId);

            var userRow = dbContext.TournamentUsers
                .Where(x => x.TournamentId == tourId)
                .Where(x => x.UserId == userId)
                .FirstOrDefault();

            if (userRow == null)
                filterContext.Result = new HttpUnauthorizedResult();

            dbContext.Dispose();
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                {
                    {"controller", "Home" },
                    {"action", "Index" },
                    {"area", "" }
                });
        }
    }
}