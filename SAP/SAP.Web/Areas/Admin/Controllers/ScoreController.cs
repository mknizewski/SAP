using SAP.BOL.Abstract;
using SAP.Web.Areas.Admin.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SAP.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ScoreController : Controller
    {
        private IScoreManager _scoreManager;

        public ScoreController(IScoreManager scoreManager)
        {
            _scoreManager = scoreManager;
        }

        public ActionResult Index(int tournamentId)
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

        #region Helper

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _scoreManager.Dispose();
                _scoreManager = null;

                base.Dispose(disposing);
            }
        }

        #endregion
    }
}