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
        private IScoreManager _scoreManager;

        public TournamentController(ITournamentManager tournamentManager, IScoreManager scoreManager)
        {
            _tournamentManager = tournamentManager;
            _scoreManager = scoreManager;
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

                var tour = new ActualTournamentsViewModel
                {
                    Id = actualT.Id,
                    Title = actualT.Title,
                };

                actualList.Add(tour);
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
                        Id = historyT.Id,
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

        public ActionResult CurrentScores(int tournamentId)
        {
            var viewModel = new ScoresViewModel();
            var currentScoresViewModel = new PhaseScoresViewModel();
            var historyScoresViewModel = new List<PhaseScoresViewModel>();

            var dbCurrentScores = _scoreManager.Scores
                .Where(x => x.TournamentId == tournamentId)
                .OrderByDescending(x => x.TotalScore)
                .ToList();

            var currentPersonalScoresViewModel = new List<PersonScoreViewModel>();
            dbCurrentScores.ForEach(x =>
            {
                currentScoresViewModel.PhaseTitle = x.Phase.Name;
                PersonScoreViewModel personScoreViewModel = new PersonScoreViewModel
                {
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    Score = x.TotalScore
                };

                currentPersonalScoresViewModel.Add(personScoreViewModel);
            });

            currentScoresViewModel.Scores = currentPersonalScoresViewModel;

            var dbHistoryScores = _scoreManager.HistoryScores
                .Where(x => x.TournamentId == tournamentId)
                .OrderByDescending(x => x.TotalScore)
                .GroupBy(x => x.PhaseId)
                .ToList();

            foreach (var phase in dbHistoryScores)
            {
                var historyPhase = new PhaseScoresViewModel();
                var personalScoresList = new List<PersonScoreViewModel>();

                foreach (var personalScore in phase)
                {
                    historyPhase.PhaseTitle = personalScore.Phase.Name;

                    var person = new PersonScoreViewModel
                    {
                        FirstName = personalScore.User.FirstName,
                        LastName = personalScore.User.LastName,
                        Score = personalScore.TotalScore
                    };

                    personalScoresList.Add(person);
                }

                historyPhase.Scores = personalScoresList;
                historyScoresViewModel.Add(historyPhase);
            }

            viewModel.CurrentPhase = currentScoresViewModel;
            viewModel.HistoryPhases = historyScoresViewModel;

            return View(viewModel);
        }

        public ActionResult HistoryScores(int tournamentId)
        {
            var viewModel = new ScoresViewModel();
            var historyScoresViewModel = new List<PhaseScoresViewModel>();

            var dbHistoryScores = _scoreManager.HistoryScores
                .Where(x => x.TournamentId == tournamentId)
                .OrderByDescending(x => x.TotalScore)
                .GroupBy(x => x.PhaseId)
                .ToList();

            foreach (var phase in dbHistoryScores)
            {
                var historyPhase = new PhaseScoresViewModel();
                var personalScoresList = new List<PersonScoreViewModel>();

                foreach (var personalScore in phase)
                {
                    var person = new PersonScoreViewModel
                    {
                        FirstName = personalScore.User.FirstName,
                        LastName = personalScore.User.LastName,
                        Score = personalScore.TotalScore
                    };

                    personalScoresList.Add(person);
                }

                historyPhase.Scores = personalScoresList;
                historyScoresViewModel.Add(historyPhase);
            }

            viewModel.HistoryPhases = historyScoresViewModel;

            return View(viewModel);
        }

        #region Helpers

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _tournamentManager.Dispose();
                _tournamentManager = null;

                _scoreManager.Dispose();
                _scoreManager = null;

                base.Dispose(disposing);
            }
        }

        #endregion Helpers
    }
}