using Microsoft.AspNet.Identity.Owin;
using SAP.BOL.Abstract;
using SAP.BOL.HelperClasses;
using SAP.BOL.LogicClasses;
using SAP.DAL.DbContext;
using SAP.DAL.DbContext.SAP;
using SAP.DAL.Tables;
using SAP.Web.Areas.Admin.Models;
using SAP.Web.HTMLHelpers;
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
        private ApplicationUserManager _userManager;

        public ManageTournamentsController(ITournamentManager tournamentManager)
        {
            _tournamentManager = tournamentManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EditTournamentList()
        {
            var viewModel = new List<TournamentListViewModel>();
            var dbTournaments = _tournamentManager.Tournaments
                .ToList();

            dbTournaments.ForEach(x =>
            {
                var tour = new TournamentListViewModel
                {
                    TournamentId = x.Id,
                    Title = x.Title
                };

                viewModel.Add(tour);
            });

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult EditTournament(int tournamentId)
        {
            var dbModel = _tournamentManager.Tournaments
                .Where(x => x.Id == tournamentId)
                .FirstOrDefault();

            var viewModel = new EditTournamentViewModel
            {
                Id = dbModel.Id,
                Title = dbModel.Title,
                Description = dbModel.Description,
                StartDate = dbModel.StartDate.Date,
                EndDate = dbModel.EndDate.Date,
                StartTime = dbModel.StartDate.TimeOfDay,
                EndTime = dbModel.EndDate.TimeOfDay,
                MaxUsers = dbModel.MaxUsers,
                IsActive = dbModel.IsActive,
                IsConfigured = dbModel.IsConfigured
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTournament(EditTournamentViewModel viewModel)
        {
            var tour = new SAP.DAL.Tables.Tournament
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                Description = viewModel.Description,
                StartDate = viewModel.StartDate.Add(viewModel.StartTime),
                EndDate = viewModel.EndDate.Add(viewModel.EndTime),
                MaxUsers = viewModel.MaxUsers
            };

            bool result = _tournamentManager.EditTournament(tour);

            if (result)
                TempData["Alert"] = SetAlert.Set("Poprawnie zmodfyfikowano turniej " + viewModel.Title, "Sukces", AlertType.Success);
            else
                TempData["Alert"] = SetAlert.Set("Wystąpił błąd, prosimy spróbować później", "Błąd", AlertType.Danger);

            return RedirectToAction("Index");
        }

        public ActionResult TournamentUsers(int tournamentId)
        {
            var usersActive = _tournamentManager.TournamentsUsers
                .Where(x => x.TournamentId == tournamentId)
                .ToList();

            var usersHistory = _tournamentManager.HistoryTournamentsUsers
                .Where(x => x.TournamentId == tournamentId)
                .ToList();

            var viewModel = new ListOfUsersViewModel();
            var listOfActiveUsers = new List<PersonScoreViewModel>();
            var listOfHistoryUsers = new List<PersonScoreViewModel>();

            usersActive.ForEach(x =>
            {
                var user = new PersonScoreViewModel
                {
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName
                };

                listOfActiveUsers.Add(user);
            });

            usersHistory.ForEach(x =>
            {
                ApplicationUser u = UserManager.FindByIdAsync(x.UserId).Result;

                var user = new PersonScoreViewModel
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName
                };

                listOfHistoryUsers.Add(user);
            });

            viewModel.ActiveUsers = listOfActiveUsers;
            viewModel.HistoryUsers = listOfHistoryUsers;

            return View(viewModel);
        }

        public ActionResult TournamentList()
        {
            var viewModel = new List<TournamentListViewModel>();
            var dbTournaments = _tournamentManager.Tournaments
                .Where(x => x.IsConfigured)
                .ToList();

            dbTournaments.ForEach(x =>
            {
                var tour = new TournamentListViewModel
                {
                    TournamentId = x.Id,
                    Title = x.Title
                };

                viewModel.Add(tour);
            });

            return View(viewModel);
        }

        public ActionResult Manage(int tournamentId)
        {
            var viewModel = new ManageTourViewModel();
            var tourDb = _tournamentManager.Tournaments
                .Where(x => x.Id == tournamentId)
                .FirstOrDefault();

            viewModel.TournamentId = tournamentId;
            viewModel.IsActive = tourDb.IsActive;
            viewModel.StartDate = tourDb.StartDate;
            viewModel.EndDate = tourDb.EndDate;
            viewModel.Title = tourDb.Title;

            var phases = new List<ManagePhaseViewModel>();
            var dbPhases = _tournamentManager.Phases
                .Where(x => x.TournamentId == tournamentId)
                .ToList();

            dbPhases.ForEach((Action<Phase>)(x =>
            {
                var phase = new ManagePhaseViewModel();
                phase.PhaseId = x.Id;
                phase.Title = x.Name;
                phase.IsActive = x.IsActive;

                var tasks = new List<MTaskViewModel>();
                var dbTasksPerPhase = _tournamentManager.Tasks
                    .Where(y => y.PhaseId == x.Id)
                    .ToList();

                dbTasksPerPhase.ForEach((Action<Tasks>)(y =>
                {
                    var task = new MTaskViewModel
                    {
                        TaskId = y.Id,
                        Title = y.Title,
                        StartDate = y.StartDate,
                        EndDate = y.EndDate,
                        IsActive = y.IsActive
                    };

                    tasks.Add(task);
                }));

                phase.Tasks = tasks;
                phases.Add(phase);
            }));

            viewModel.Phases = phases;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SetTourFlag(int tournamentId, bool flag)
        {
            _tournamentManager.SetTournamentActiveFlag(tournamentId, flag);

            TempData["Alert"] = SetAlert.Set("Poprawnie wykonano działanie!", "Sukces", AlertType.Success);
            return RedirectToAction("Manage", new { @tournamentId = tournamentId });
        }

        [HttpPost]
        public ActionResult SetTaskFlag(int taskId, int tournamentId, bool flag)
        {
            _tournamentManager.SetTaskActiveFlag(taskId, flag);

            TempData["Alert"] = SetAlert.Set("Poprawnie wykonano działanie!", "Sukces", AlertType.Success);
            return RedirectToAction("Manage", new { @tournamentId = tournamentId });
        }

        public ActionResult SetPhaseFlag(int phaseId, int tournamentId, bool flag)
        {
            _tournamentManager.SetPhaseActiveFlag(phaseId, flag);

            TempData["Alert"] = SetAlert.Set("Poprawnie wykonano działanie!", "Sukces", AlertType.Success);
            return RedirectToAction("Manage", new { @tournamentId = tournamentId });
        }

        [HttpPost]
        public ActionResult CountScores(int tournamentId, int phaseId)
        {
            _tournamentManager.CountScores(tournamentId, phaseId);

            TempData["Alert"] = SetAlert.Set("Poprawnie wykonano działanie!", "Sukces", AlertType.Success);
            return RedirectToAction("Manage", new { @tournamentId = tournamentId });
        }

        [HttpPost]
        public ActionResult SetPromotions(int tournamentId, int phaseId)
        {
            _tournamentManager.SetPromotions(tournamentId, phaseId);

            TempData["Alert"] = SetAlert.Set("Poprawnie wykonano działanie!", "Sukces", AlertType.Success);
            return RedirectToAction("Manage", new { @tournamentId = tournamentId });
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
                        var task = new Tasks
                        {
                            Title = y.Title,
                            Order = y.Order,
                            Description = y.Description,
                            EndDate = y.EndDate.Add(y.EndTime),
                            StartDate = y.StartDate.Add(y.StartTime),
                            Input = y.Input,
                            Output = y.Output,
                            Example = y.Example,
                            MaxExecuteMemory = y.MaxExecutedMemory,
                            MaxExecuteTime = y.MaxExecutedTime,
                            InputDataTypeId = y.InputDataId
                        };

                        if (y.PDF != null)
                        {
                            byte[] pdf = new byte[y.PDF.ContentLength];
                            y.PDF.InputStream.Read(pdf, 0, y.PDF.ContentLength);
                            task.PDF = pdf;
                        }

                        tasks.Add(task);
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
                var taskList = new List<ManageTaskViewModel>();

                for (int j = 0; j < taskCount[i]; j++)
                    taskList.Add(new ManageTaskViewModel { PhaseId = i, InputData = GetDataInputList(), StartTime = timeNow, EndTime = timeNow, StartDate = dateNow, EndDate = dateNow });

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

        public ActionResult ManageTestData()
        {
            var tournamnets = _tournamentManager.Tournaments;

            List<SelectListItem> tourList = new List<SelectListItem>();
            foreach (var item in tournamnets)
                tourList.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Title });

            ViewBag.TourList = tourList;
            return View();
        }

        public ActionResult GetTestData(int taskId)
        {
            var dbModel = _tournamentManager.TasksTestData
                .Where(x => x.TaskId == taskId)
                .ToList();

            var viewModel = new List<TestDataViewModel>();

            dbModel.ForEach(x =>
            {
                var model = new TestDataViewModel
                {
                    Id = x.Id,
                    TaskId = x.TaskId,
                    InputData = x.InputData,
                    OutputData = x.OutputData
                };

                viewModel.Add(model);
            });

            return PartialView("~/Areas/Admin/Views/Shared/TestDataView.cshtml", viewModel);
        }

        public JsonResult DeleteTestData(int testId)
        {
            bool result = _tournamentManager.DeleteTestData(testId);

            if (result)
            {
                var alert = SetAlert.Set("Poprawnie usunięto dane testowe!", "Sukces", AlertType.Success);
                return Json(Alert.GetAlert(alert).ToHtmlString());
            }
            else
            {
                var alert = SetAlert.Set("Wystąpił błąd, spróbuj ponownie później", "Błąd", AlertType.Danger);
                return Json(Alert.GetAlert(alert).ToHtmlString());
            }
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
            //var todayTask = TodoManager.todayTasks;

            //List<TodaySystemTaskViewModel> viewModel = new List<TodaySystemTaskViewModel>();

            //todayTask.ForEach(x =>
            //{
            //    TodaySystemTaskViewModel task = new TodaySystemTaskViewModel();

            //    switch (x.TaskType)
            //    {
            //        case TaskType.ScoreCount:
            //            task.TournamentId = x.TournamentId;
            //            task.PhaseId = x.PhaseId;
            //            task.TypeOfTask = "Podsumowanie wyników";
            //            break;
            //        case TaskType.SetPromotions:
            //            task.TournamentId = x.TournamentId;
            //            task.PhaseId = x.PhaseId;
            //            task.TypeOfTask = "Przydzielenie awansów";
            //            break;
            //        default:
            //            task.TaskId = x.TaskId;
            //            task.TypeOfTask = x.TaskType == TaskType.StartPhase ? "Start fazy"
            //            : x.TaskType == TaskType.EndPhase ? "Koniec fazy"
            //            : x.TaskType == TaskType.StartTask ? "Start zadania"
            //            : x.TaskType == TaskType.EndTask ? "Koniec zadania"
            //            : x.TaskType == TaskType.StartTournament ? "Początek turnieju"
            //            : x.TaskType == TaskType.EndTournament ? "Koniec turnieju"
            //            : String.Empty;
            //            break;
            //    }

            //    task.ExecuteTime = x.ExecuteTime;
            //    task.IsRealized = x.IsRealized;
            //    task.TaskType = x.TaskType;

            //    viewModel.Add(task);
            //});

            //ViewBag.LastSynchro = TodoManager.LastSynchronized;

            //return View(viewModel);

            TempData["Alert"] = SetAlert.Set("Opcja w tej wersji jest niedostępna.", "Uwaga", AlertType.Warning);
            return RedirectToAction("Index");
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
            TodoManager.InicializeTodayTasks();

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

                if (activePhase != null && activeTask != null)
                {
                    var itemViewModel = new TournamentsCourseViewModel
                    {
                        TourId = item.Id,
                        TourTitle = item.Title,
                        ActivePhaseTitle = activePhase.Name,
                        ActiveTaskTitle = activeTask.Title
                    };

                    viewModel.Add(itemViewModel);
                }
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

        public ActionResult CourseSaveChanges(int tourId, int phaseId, int taskId)
        {
            bool result = _tournamentManager.CourseSaveChanges(tourId, phaseId, taskId);

            if (result)
            {
                var alert = SetAlert.Set("Poprawnie zmieniono przebieg turnieju o id " + tourId + "!", "Sukces", AlertType.Success);
                return Json(Alert.GetAlert(alert).ToHtmlString());
            }
            else
            {
                var alert = SetAlert.Set("Wystąpił błąd, spróbuj ponownie później", "Błąd", AlertType.Danger);
                return Json(Alert.GetAlert(alert).ToHtmlString());
            }
        }

        public ActionResult CourseTaskDetails(int phaseId, int tourId)
        {
            var selectListItem = new List<SelectListItem>();
            var taskDb = _tournamentManager.Tasks
                .Where(x => x.TournamentId == tourId)
                .Where(x => x.PhaseId == phaseId);

            foreach (var item in taskDb)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = item.Title,
                    Value = item.Id.ToString()
                });
            }

            return Json(selectListItem);
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

        private IEnumerable<SelectListItem> GetDataInputList()
        {
            List<SelectListItem> dataList = new List<SelectListItem>();

            dataList.Add(new SelectListItem { Text = "Argumenty wywołania", Value = ((int)(InputDataType.Arguments)).ToString() });
            dataList.Add(new SelectListItem { Text = "Strumień danych", Value = ((int)(InputDataType.Stream)).ToString() });
            dataList.Add(new SelectListItem { Text = "Brak", Value = ((int)(InputDataType.None)).ToString() });

            return dataList;
        }

        #endregion Helpers
    }
}