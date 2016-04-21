using Microsoft.AspNet.Identity;
using SAP.BOL.Abstract;
using SAP.BOL.HelperClasses;
using SAP.Web.Areas.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SAP.Web.Areas.Tournament.Controllers
{
    public class HomeController : Controller
    {
        private ITournamentManager _tournamentManager;

        public HomeController(ITournamentManager tournamentManager)
        {
            _tournamentManager = tournamentManager;
        }

        [HttpGet]
        public ActionResult Index(int page = 1)
        {
            var tour = _tournamentManager.GetTourByPage(page);
            var viewModel = new IndexInfoViewModel();
            var listOfTour = new List<HomeTournamentViewModel>();

            viewModel.CurrentPage = page;
            viewModel.TotalPages = (int)Math.Ceiling((decimal)tour.TotalItems / 5);

            foreach (var item in tour.Tournaments)
            {
                var itemViewModel = new HomeTournamentViewModel
                {
                    Id = item.Id,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    Description = item.Description,
                    Title = item.Title,
                    MaxUsers = item.MaxUsers,
                    PhaseCount = _tournamentManager.Phases.Where(x => x.TournamentId == item.Id).Count()
                };

                if (item.IsActive)
                    itemViewModel.Status = TournamentStatus.Active;
                else if (!item.IsActive && item.EndDate > DateTime.Now)
                    itemViewModel.Status = TournamentStatus.Register;
                else
                    itemViewModel.Status = TournamentStatus.Ended;

                listOfTour.Add(itemViewModel);
            }

            viewModel.ViewModel = listOfTour;
            viewModel.ViewModel = viewModel.ViewModel.OrderBy(x => x.Status).ToList();
            return View(viewModel);
        }

        public ActionResult Search(string title)
        {
            var dbList = _tournamentManager.Tournaments
                .Where(x => x.Title.Contains(title))
                .ToList();

            var viewModel = new IndexInfoViewModel();
            var listOfTour = new List<HomeTournamentViewModel>();

            viewModel.CurrentPage = 1;
            viewModel.TotalPages = 1;

            foreach (var item in dbList)
            {
                var itemViewModel = new HomeTournamentViewModel
                {
                    Id = item.Id,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    Description = item.Description,
                    Title = item.Title,
                    MaxUsers = item.MaxUsers,
                    PhaseCount = _tournamentManager.Phases.Where(x => x.TournamentId == item.Id).Count()
                };

                if (item.IsActive)
                    itemViewModel.Status = TournamentStatus.Active;
                else if (!item.IsActive && item.EndDate > DateTime.Now)
                    itemViewModel.Status = TournamentStatus.Register;
                else
                    itemViewModel.Status = TournamentStatus.Ended;

                listOfTour.Add(itemViewModel);
            }

            viewModel.ViewModel = listOfTour;
            viewModel.ViewModel = viewModel.ViewModel.OrderBy(x => x.Status).ToList();

            return View("Index", viewModel);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult Register(int tourId)
        {
            string userId = User.Identity.GetUserId();
            bool result = _tournamentManager.RegisterToTournament(userId, tourId);

            if (result)
                TempData["Alert"] = SetAlert.Set("Poprawnie zapisano do turnieju!", "Sukces", AlertType.Success);
            else
                TempData["Alert"] = SetAlert.Set("W danym turnieju wykorzystano limit miejsc!", "Błąd", AlertType.Danger);

            return RedirectToAction("Index");
        }

        public ActionResult Details(int tourId)
        {
            return View();
        }

        #region Helpers

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _tournamentManager.Dispose();
                base.Dispose(disposing);
            }
        }

        #endregion Helpers
    }

    public enum TournamentStatus
    {
        Register, Active, Ended
    }
}