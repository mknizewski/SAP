using SAP.BOL.Abstract;
using SAP.BOL.HelperClasses;
using SAP.BOL.LogicClasses;
using SAP.DAL.Tables;
using SAP.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                    StartDate = viewModel.Tournament.StartDate.Add(viewModel.Tournament.StartTime),
                    EndDate = viewModel.Tournament.EndDate.Add(viewModel.Tournament.EndTime),
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
                        Order = x.Order,
                        MaxTasks = x.TaskCount
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
                            EndDate = y.EndDate.Add(y.EndTime),
                            StartDate = y.StartDate.Add(y.StartTime),
                            ExampleInput = y.ExampleInput,
                            ExampleOutput = y.ExampleOutput
                        });
                    });
                });

                bool result = await _tournamentManager.AddTournamnetAsync(tour, phases, tasks, taskCountPerPhase);

                if (result)
                {
                    TempData["Alert"] = SetAlert.Set("Poprawnie dodano turniej! Teraz skonfiguruj dane testowe do poszczególnych zadań, by móc poprawnie aktywować turniej.", "Sukces", AlertType.Info);
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
            TimeSpan timeNow = TimeSpan.MinValue;
            DateTime dateNow = DateTime.Now;

            for (int i = 0; i < taskCount.Length; i++)
            {
                var taskList = new List<TaskViewModel>();

                for (int j = 0; j < taskCount[i]; j++)
                    taskList.Add(new TaskViewModel { PhaseId = i, InputData = GetDataInputList(), StartTime = timeNow, EndTime = timeNow, StartDate = dateNow, EndDate = dateNow });

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
        public async Task<ActionResult> AddTestDataTask(List<TestDataViewModel> viewModel)
        {
            if (ModelState.IsValid)
            {
                List<TasksTestData> listData = new List<TasksTestData>();

                foreach (var item in viewModel)
                {
                    listData.Add(new TasksTestData
                    {
                        InputData = item.InputData,
                        OutputData = item.OutputData,
                        TaskId = item.TaskId
                    });
                }

                bool result = await _tournamentManager.AddTestDataAsync(listData);

                if (result)
                {
                    TempData["Alert"] = SetAlert.Set("Poprawnie wprowadzono dane testowe!", "Sukces", AlertType.Success);
                    return RedirectToAction("AddTestDataTask");
                }
                else
                {
                    TempData["Alert"] = SetAlert.Set("Wystąpił błąd. Spróbuj ponownie później", "Błąd", AlertType.Danger);
                    return RedirectToAction("AddTestDataTask");
                }
            }
            else
            {
                var tournamnets = _tournamentManager.Tournaments
                .Select(x => x)
                .Where(x => x.IsConfigured == false); //zwraca nieskonfigurowane turnieje

                List<SelectListItem> tourList = new List<SelectListItem>();
                foreach (var item in tournamnets)
                    tourList.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Title });

                TempData["Alert"] = SetAlert.Set("Sprawdź poprawnośc danych!", "Błąd", AlertType.Danger);
                return View(viewModel);
            }
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

        public ActionResult GetTestDataModel(int taskId, int taskCount)
        {
            var viewModel = new List<TestDataViewModel>();

            for (int i = 0; i < taskCount; i++)
                viewModel.Add(new TestDataViewModel { TaskId = taskId });

            return PartialView("~/Areas/Admin/Views/Shared/TestDataModel.cshtml", viewModel);
        }

        public ActionResult TodaySystemTask()
        {
            var todayTask = TodoList.todayTasks;

            List<TodaySystemTaskViewModel> viewModel = new List<TodaySystemTaskViewModel>();

            todayTask.ForEach(x => 
            {
                TodaySystemTaskViewModel task = new TodaySystemTaskViewModel();
                task.Id = x.Id;
                task.ExecuteTime = x.ExecuteTime;
                task.IsRealized = x.IsRealized;
                task.TypeOfTask = x.TaskType == TaskType.StartPhase ? "Start fazy"
                : x.TaskType == TaskType.EndPhase ? "Koniec fazy"
                : x.TaskType == TaskType.StartTask ? "Start zadania"
                : x.TaskType == TaskType.EndTask ? "Koniec zadania"
                : x.TaskType == TaskType.StartTournament ? "Początek turnieju"
                : x.TaskType == TaskType.EndTournament ? "Koniec turnieju"
                : String.Empty;

                viewModel.Add(task);
            });

            ViewBag.LastSynchro = TodoList.LastSynchronized;

            return View(viewModel);
        }

        public ActionResult Configuration()
        {
            List<TournamentsViewModel> viewModel = new List<TournamentsViewModel>();
            var tournamentsList = _tournamentManager.Tournaments
                .Where(x => !x.IsConfigured)
                .ToList();

            tournamentsList.ForEach(x => viewModel.Add(new TournamentsViewModel
            {
                Id = x.Id,
                Title = x.Title,
                StartDate = x.StartDate
            }));

            return View(viewModel);
        }

        public ActionResult Synchronize()
        {
            TodoList.InicializeTodayTasks();

            return RedirectToAction("TodaySystemTask");
        }

        public JsonResult ValidateTournament(int Id)
        {
            var errorList = _tournamentManager.ValidateTournament(Id);
            return Json(errorList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetConfigure(int Id)
        {
            bool result = _tournamentManager.ConfigureSet(Id, true);

            TempData["Alert"] = SetAlert.Set("Turniej o id: " + Id + " został skonfigurowany.", "Sukcess", AlertType.Success);
            return Json(result);
        }

        public ActionResult TournamentsCourse()
        {
            var viewModel = new List<TournamentsCourseViewModel>();
            var dbTour = _tournamentManager.Tournaments
                .Where(x => x.IsActive);

            foreach (var item in dbTour)
            {
                var activePhase = _tournamentManager.Phases
                    .Where(x => x.TournamentId == item.Id)
                    .Where(x => x.IsActive)
                    .FirstOrDefault();

                var activeTask = _tournamentManager.Tasks
                    .Where(x => x.TournamentId == item.Id)
                    .Where(x => x.IsActive)
                    .FirstOrDefault();

                var itemViewModel = new TournamentsCourseViewModel
                {
                    TourId = item.Id,
                    TourTitle = item.Title,
                    ActivePhaseTitle = activePhase.Name,
                    ActiveTaskTitle = activeTask.Title
                };

                viewModel.Add(itemViewModel);
            }

            return View(viewModel);
        }

        public ActionResult CoursePhaseDetails(int id)
        {
            var selectListItem = new List<SelectListItem>();
            var phaseDb = _tournamentManager.Phases
                .Where(x => x.TournamentId == id);

            foreach (var item in phaseDb)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }

            return Json(selectListItem);
        }

        public ActionResult CourseTaskDetails(int phaseId)
        {
            return Json(null);
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

        private IEnumerable<SelectListItem> GetDataInputList()
        {
            List<SelectListItem> dataList = new List<SelectListItem>();

            dataList.Add(new SelectListItem { Text = "Argumenty wywołania", Value = InputDataType.Arguments.ToString() });
            dataList.Add(new SelectListItem { Text = "Strumień danych", Value = InputDataType.Stream.ToString() });
            dataList.Add(new SelectListItem { Text = "Brak", Value = InputDataType.None.ToString() });

            return dataList;
        }

        #endregion Helpers
    }
}