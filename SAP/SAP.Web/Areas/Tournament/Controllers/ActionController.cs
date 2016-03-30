using SAP.BOL.Abstract;
using SAP.BOL.LogicClasses;
using SAP.Web.Areas.Tournament.Models;
using SAP.Web.Infrastructrue.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SAP.Web.Areas.Tournament.Controllers
{
    [Authorize(Roles = "User")]
    public class ActionController : Controller
    {
        private ITournamentManager _tournamentManager;
        private IProgramManager _programManager;

        public ActionController(ITournamentManager tournamentManager, IProgramManager programManager)
        {
            _tournamentManager = tournamentManager;
            _programManager = programManager;
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

        public ActionResult Task(int taskId, TaskGetType type)
        {
            ViewBag.Compiler = GetCompilersList();
            ViewBag.Type = type;
            var viewModel = _tournamentManager.Tasks
                .Where(x => x.Id == taskId)
                .FirstOrDefault();

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Solution(SolutionViewModel viewModel)
        {
            await System.Threading.Tasks.Task.Run(() => 
            {
                
            });

            return View();
        }

        #region Helpers

        public enum TaskGetType
        { Detials, Solution }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _tournamentManager.Dispose();
                _tournamentManager = null;

                _programManager.Dispose();
                _programManager = null;

                base.Dispose(disposing);
            }
        }

        private List<SelectListItem> GetCompilersList()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem { Text = "C", Value = CompilerType.C.ToString() });
            list.Add(new SelectListItem { Text = "C++", Value = CompilerType.Cpp.ToString() });
            list.Add(new SelectListItem { Text = "Java", Value = CompilerType.Java.ToString() });
            list.Add(new SelectListItem { Text = "Pascal", Value = CompilerType.Pascal.ToString() });

            return list;
        }
        #endregion
    }
}