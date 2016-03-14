using SAP.BOL.Abstract;
using SAP.BOL.HelperClasses;
using SAP.DAL.Tables;
using SAP.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SAP.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageTournamentsController : Controller
    {
        private ITournamentManager _tournamentManager;

        public ManageTournamentsController(ITournamentManager tournamentManager)
        {
            _tournamentManager = tournamentManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddTournament()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddTournament(AddTournamentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                int[] taskCountPerPhase = new int[viewModel.Phases.Count];
                for (int i = 0; i < taskCountPerPhase.Length; i++)
                    taskCountPerPhase[i] = viewModel.TaskContainer[i].Tasks.Count;

                DAL.Tables.Tournament tour = new DAL.Tables.Tournament
                {
                    Title = viewModel.Tournament.Title,
                    Description = viewModel.Tournament.Description,
                    StartDate = viewModel.Tournament.StartDate,
                    EndDate = viewModel.Tournament.EndDate,
                    MaxExecuteMemory = viewModel.Tournament.MaxExecutedMemory,
                    MaxExecuteTime = viewModel.Tournament.MaxExecutedTime,
                    MaxUsers = viewModel.Tournament.MaxUsers
                };

                List<Phase> phases = new List<Phase>();

                viewModel.Phases.ForEach(x => 
                {
                    phases.Add(new Phase
                    {
                        MaxUsers = x.MaxUsers,
                        Name = x.Name,
                        Order= x.Order
                    });
                });

                List<Tasks> tasks = new List<Tasks>();

                viewModel.TaskContainer.ForEach(x => 
                {
                    x.Tasks.ForEach(y => 
                    {
                        tasks.Add(new Tasks
                        {
                            Title = y.Title,
                            Order = y.Order,
                            Description = y.Description,
                            EndDate = y.EndDate,
                            StartDate = y.StartDate,
                            ExampleInput = y.ExampleInput,
                            ExampleOutput = y.ExampleOutput
                        });
                    });
                });

                bool result = await _tournamentManager.AddTournamnetAsync(tour, phases, tasks, taskCountPerPhase); 

                if (result)
                {
                    TempData["Alert"] = SetAlert.Set("Poprawnie dodano turniej! Teraz skonfiguruj dane testowe do poszczególnych zadań, by system mógł poprawnie aktywować turniej.", "Sukces", AlertType.Info);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Alert"] = SetAlert.Set("Wystąpił błąd z bazą danych. Spróbuj ponownie później.", "Błąd", AlertType.Danger);
                    return View(viewModel);
                }
                
            }
            else
            {
                TempData["Alert"] = SetAlert.Set("Wystąpił błąd, proszę sprawdź wszystkie kroki.", "Błąd!", AlertType.Danger);
                return View(viewModel);
            }
        }

        public ActionResult ConfigurePhase(int count)
        {
            var phaseList = new List<PhaseViewModel>();

            for (int i = 0; i < count; i++)
                phaseList.Add(new PhaseViewModel());

            return PartialView("~/Areas/Admin/Views/Shared/PhaseList.cshtml", phaseList);
        }

        public ActionResult ConfigureTask(int[] taskCount)
        {
            var taskContainer = new List<PhaseTaskContainer>();

            for (int i = 0; i < taskCount.Length; i++)
            {
                var taskList = new List<TaskViewModel>();

                for (int j = 0; j < taskCount[i]; j++)
                    taskList.Add(new TaskViewModel { PhaseId = i });

                taskContainer.Add(new PhaseTaskContainer { Tasks = taskList });
            }

            return PartialView("~/Areas/Admin/Views/Shared/TaskList.cshtml", taskContainer);
        }

        public ActionResult AddTestDataTask()
        {
            var tournamnets = _tournamentManager.Tournaments
                .Select(x => x)
                .Where(x => x.IsConfigured == false); //zwraca nieskonfigurowane turnieje

            List<SelectListItem> tourList = new List<SelectListItem>();
            foreach (var item in tournamnets)
                tourList.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Title });

            ViewBag.TourList = tourList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTestDataTask(TestDataViewModel viewModel)
        {
            return View();
        }

        public ActionResult GetPhaseById(int tournamentId)
        {
            var phases = _tournamentManager.Phases
                .Select(x => x)
                .Where(x => x.TournamentId == tournamentId);

            var jsonList = new List<SelectListItem>();

            foreach (var item in phases)
                jsonList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });

            return Json(jsonList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTaskById(int phaseId, int tournamentId)
        {
            var tasks = _tournamentManager.Tasks
                .Select(x => x)
                .Where(x => x.TournamentId == tournamentId)
                .Where(x => x.PhaseId == phaseId);

            var jsonList = new List<SelectListItem>();

            foreach (var item in tasks)
                jsonList.Add(new SelectListItem { Text = item.Title, Value = item.Id.ToString() });

            return Json(jsonList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTestDataModel(int taskId)
        {
            var viewModel = new TestDataViewModel
            {
                TaskId = taskId
            };

            return PartialView("~/Areas/Admin/Views/Shared/TestDataModel.cshtml", viewModel);
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

        #endregion
    }
}