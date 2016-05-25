using SAP.BOL.Abstract;
using SAP.BOL.HelperClasses;
using SAP.BOL.LogicClasses;
using SAP.DAL.Tables;
using SAP.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace SAP.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageTaskController : Controller
    {
        private ITournamentManager _tournamentManager;
        private IUserManager _userManager;

        public ManageTaskController(ITournamentManager tournamentManager, IUserManager userManager)
        {
            _tournamentManager = tournamentManager;
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            var viewModel = _tournamentManager.Tasks
                .ToList();

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult EditTask(int taskId)
        {
            var dbModel = _tournamentManager.Tasks
                .Where(x => x.Id == taskId)
                .FirstOrDefault();

            var viewModel = new ManageTaskViewModel
            {
                Title = dbModel.Title,
                Output = dbModel.Output,
                Input = dbModel.Input,
                InputDataId = dbModel.InputDataTypeId,
                Example = dbModel.Example,
                EndDate = dbModel.EndDate.Date,
                EndTime = dbModel.EndDate.TimeOfDay,
                StartDate = dbModel.StartDate.Date,
                StartTime = dbModel.StartDate.TimeOfDay,
                Description = dbModel.Description,
                MaxExecutedMemory = dbModel.MaxExecuteMemory,
                MaxExecutedTime = dbModel.MaxExecuteTime,
                TaskId = dbModel.Id,
                InputData = GetDataInputList(dbModel.InputDataTypeId)
            };

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditTask(ManageTaskViewModel model)
        {
            var dbModel = new Tasks
            {
                Title = model.Title,
                Description = model.Description,
                StartDate = model.StartDate.Add(model.StartTime),
                EndDate = model.EndDate.Add(model.EndTime),
                Example = model.Example,
                Input = model.Input,
                Id = model.TaskId,
                InputDataTypeId = model.InputDataId,
                Output = model.Output,
                MaxExecuteMemory = model.MaxExecutedMemory,
                MaxExecuteTime = model.MaxExecutedTime
            };

            if (model.PDF != null)
            {
                byte[] newPDF = new byte[model.PDF.ContentLength];
                model.PDF.InputStream.Read(newPDF, 0, model.PDF.ContentLength);

                dbModel.PDF = newPDF;
            }

            bool result = _tournamentManager.EditTask(dbModel);

            if (result)
                TempData["Alert"] = SetAlert.Set("Poprawnie edytowano: " + dbModel.Title, "Sukces", AlertType.Success);
            else
                TempData["Alert"] = SetAlert.Set("Wystąpił błąd. Spróbuj ponownie później!", "Błąd", AlertType.Danger);

            return RedirectToAction("Index");
        }

        public FileResult GetPDF(int taskId)
        {
            var binaryPdf = _tournamentManager.Tasks
                .Where(x => x.Id == taskId)
                .Select(x => x.PDF)
                .FirstOrDefault();

            return File(binaryPdf, "application/pdf", "zadanie.pdf");
        }

        public ActionResult AddTask()
        {
            //TODO: DOKOŃCZYC
            return null;
        }

        public ActionResult UserSolutions(string userId)
        {
            var viewModel = new List<SendSolutionViewModel>();
            var dbModel = _userManager.Solutions
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.InsertTime)
                .ToList();

            dbModel.ForEach(x =>
            {
                var model = new SendSolutionViewModel
                {
                    UserName = x.User.UserName,
                    TournamentTitle = x.Tournament.Title,
                    TaskTitle = x.Task.Title,
                    InsertTime = x.InsertTime,
                    SolutionId = x.Id,
                    IsAccepted = x.Score == 0 ? false : true,
                    Lang = ((CompilerType)x.CompilerId).ToString(),
                    Error = x.Error,
                    MemUsage = x.MemoryUsage,
                    TimeUsage = x.ExecutedTime
                };

                viewModel.Add(model);
            });

            return View(viewModel);
        }

        public FileResult GetSolutionFile(int solutionId)
        {
            var solution = _userManager.Solutions
                .Where(x => x.Id == solutionId)
                .FirstOrDefault();

            string program = String.IsNullOrEmpty(solution.Program) ? String.Empty : solution.Program;
            CompilerType compilerType = (CompilerType)solution.CompilerId;
            string fileName = solution.Task.Title;

            fileName = fileName.ToLower();
            fileName = fileName.Replace(' ', '_');

            byte[] binaryProgram = Encoding.ASCII.GetBytes(program);

            switch (compilerType)
            {
                case CompilerType.C:
                    fileName += "-" + solutionId + ".c";
                    break;

                case CompilerType.Cpp:
                    fileName += "-" + solutionId + ".cpp";
                    break;

                case CompilerType.Java:
                    fileName += "-" + solutionId + ".java";
                    break;

                case CompilerType.Pascal:
                    fileName += "-" + solutionId + ".pas";
                    break;
            }

            return File(binaryProgram, "text/plain", fileName);
        }

        #region Helpers

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

        private IEnumerable<SelectListItem> GetDataInputList(int activeId)
        {
            List<SelectListItem> dataList = new List<SelectListItem>();

            dataList.Add(new SelectListItem { Text = "Argumenty wywołania", Value = ((int)(InputDataType.Arguments)).ToString() });
            dataList.Add(new SelectListItem { Text = "Strumień danych", Value = ((int)(InputDataType.Stream)).ToString() });
            dataList.Add(new SelectListItem { Text = "Brak", Value = ((int)(InputDataType.None)).ToString() });

            var active = dataList
                .Where(x => x.Value == activeId.ToString())
                .FirstOrDefault();

            active.Selected = true;

            return dataList;
        }

        #endregion Helpers
    }
}