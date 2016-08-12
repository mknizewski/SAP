using Microsoft.AspNet.Identity;
using SAP.BOL.Abstract;
using SAP.BOL.HelperClasses;
using SAP.BOL.LogicClasses;
using SAP.Web.Areas.Tournament.Models;
using SAP.Web.Infrastructrue.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Web.Mvc;

namespace SAP.Web.Areas.Tournament.Controllers
{
    [Authorize(Roles = "User")]
    public class ActionController : Controller
    {
        private ITournamentManager _tournamentManager;
        private IUserManager _userManager;

        public ActionController(ITournamentManager tournamentManager, IUserManager userManager)
        {
            _tournamentManager = tournamentManager;
            _userManager = userManager;
        }

        [TournamentsAuthorization]
        public ActionResult Index(int tourId)
        {
            var viewModel = new ActionViewModel();
            var phases = new List<PhasesViewModel>();

            var phaseDb = _tournamentManager.GetActiveAndEndPhase(tourId);
            var taskDb = _tournamentManager.GetActiveAndEndTask(tourId);

            viewModel.Title = _tournamentManager.Tournaments
                .Where(x => x.Id == tourId)
                .FirstOrDefault()
                .Title;

            phaseDb.ForEach(x =>
            {
                var phase = new PhasesViewModel();
                var taskList = new List<TasksViewModel>();
                phase.Title = x.Name;

                var phaseTask = taskDb
                                .Where(y => y.PhaseId == x.Id)
                                .ToList();

                phaseTask.ForEach(y =>
                {
                    var task = new TasksViewModel
                    {
                        Id = y.Id,
                        IsActive = y.IsActive,
                        Title = y.Title,
                        StartDate = y.StartDate,
                        EndDate = y.EndDate
                    };

                    taskList.Add(task);
                });

                phase.Tasks = taskList;
                phases.Add(phase);
            });

            viewModel.Phases = phases;
            return View(viewModel);
        }

        public ActionResult Task(int taskId, ActionType type)
        {
            ViewBag.Compiler = GetCompilersList();
            ViewBag.Type = type;
            var viewModel = _tournamentManager.Tasks
                .Where(x => x.Id == taskId)
                .FirstOrDefault();

            ViewBag.Task = viewModel;

            return View();
        }

        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Solution(SolutionViewModel viewModel)
        {
            string userId = User.Identity.GetUserId();
            object[] parameters;

            if (viewModel.File != null)
            {
                byte[] file = new byte[viewModel.File.ContentLength];
                viewModel.File.InputStream.Read(file, 0, file.Length);

                string program = Encoding.UTF8.GetString(file);
                parameters = new object[]
                {
                    program,
                    userId,
                    viewModel.TaskId,
                    (CompilerType)viewModel.SelectedLang,
                    viewModel.JavaMainClassName
                };
            }
            else
                parameters = new object[]
                {
                    viewModel.Program,
                    userId,
                    viewModel.TaskId,
                    (CompilerType)viewModel.SelectedLang,
                    viewModel.JavaMainClassName
                };

            HostingEnvironment.QueueBackgroundWorkItem(x =>
            {
            });

            TempData["Alert"] = SetAlert.Set("Dziękujemy za przesłanie zgłoszenia! Wynik możesz poznać w sekcji <b>Zgłoszone rozwiązania</b> w menu.", "Sukces", AlertType.Success);
            return RedirectToAction("Index", "Home", new { @area = "User" });
        }

        public FileResult GetPDF(int taskId)
        {
            var task = _tournamentManager.Tasks
                .Where(x => x.Id == taskId)
                .FirstOrDefault();

            byte[] binaryPdf = task.PDF == null ? new byte[0] : task.PDF;
            string fileName = task.Title;

            fileName = fileName.ToLower();
            fileName = fileName.Replace(' ', '_');
            fileName += ".pdf";

            return File(binaryPdf, "application/pdf", fileName);
        }

        #region Helpers

        public enum ActionType
        { Detials, Solution }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _tournamentManager.Dispose();
                _tournamentManager = null;

                _userManager.Dispose();
                _userManager = null;

                base.Dispose(disposing);
            }
        }

        private List<SelectListItem> GetCompilersList()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem { Text = "C", Value = (((int)(CompilerType.C))).ToString() });
            list.Add(new SelectListItem { Text = "C++", Value = (((int)(CompilerType.Cpp))).ToString() });
            list.Add(new SelectListItem { Text = "Java", Value = (((int)(CompilerType.Java))).ToString() });
            list.Add(new SelectListItem { Text = "Pascal", Value = (((int)(CompilerType.Pascal))).ToString() });

            return list;
        }

        #endregion Helpers
    }
}