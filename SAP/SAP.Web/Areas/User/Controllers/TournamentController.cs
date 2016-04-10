using Microsoft.AspNet.Identity;
using SAP.BOL.Abstract;
using SAP.Web.Areas.User.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SAP.Web.Areas.User.Controllers
{
    [Authorize(Roles = "User")]
    public class TournamentController : Controller
    {
        private ITournamentManager _tournamentManager;

        public TournamentController(ITournamentManager tournamentManager)
        {
            _tournamentManager = tournamentManager;
        }

        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var viewModel = new TournamentsIndexViewModel();
            var actualList = new List<ActualTournamentsViewModel>();
            var historyList = new List<HistoryTournamentsViewModel>();

            var actualTour = _tournamentManager.TournamentsUsers
                .Where(x => x.UserId == userId);

            foreach (var item in actualTour)
            {
                var actualT = _tournamentManager.Tournaments
                    .Where(x => x.Id == item.TournamentId)
                    .Where(x => x.IsActive)
                    .FirstOrDefault();

                var actualP = _tournamentManager.Phases
                    .Where(x => x.TournamentId == actualT.Id)
                    .Where(x => x.IsActive)
                    .FirstOrDefault();

                var actualTask = _tournamentManager.Tasks
                    .Where(x => x.TournamentId == actualT.Id)
                    .Where(x => x.IsActive)
                    .FirstOrDefault();

                if (actualTask != null && actualP != null)
                {
                    var tour = new ActualTournamentsViewModel
                    {
                        Id = actualT.Id,
                        Title = actualT.Title,
                        Phase = actualP.Name,
                        Task = actualTask.Title
                    };

                    actualList.Add(tour);
                }
            }

            var historyTour = _tournamentManager.HistoryTournamentsUsers
                .Where(x => x.UserId == userId);

            foreach (var item in historyTour)
            {
                var historyT = _tournamentManager.Tournaments
                    .Where(x => x.Id == item.TournamentId)
                    .FirstOrDefault();

                var historyP = _tournamentManager.Phases
                    .Where(x => x.Id == item.PhaseId)
                    .FirstOrDefault();

                if (historyT != null && historyP != null)
                {
                    var history = new HistoryTournamentsViewModel
                    {
                        Title = historyT.Title,
                        StartDate = historyT.StartDate,
                        EndDate = historyT.EndDate,
                        Phase = historyP.Name
                    };

                    historyList.Add(history);
                }
            }

            viewModel.ActualTour = actualList;
            viewModel.HistoryTour = historyList;

            return View(viewModel);
        }

        #region Helpers

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _tournamentManager.Dispose();
                _tournamentManager = null;

                base.Dispose(disposing);
            }
        }

        #endregion Helpers
    }
}