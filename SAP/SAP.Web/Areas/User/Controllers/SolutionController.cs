using Microsoft.AspNet.Identity;
using SAP.BOL.Abstract;
using SAP.BOL.LogicClasses;
using SAP.Web.Areas.User.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace SAP.Web.Areas.User.Controllers
{
    [Authorize(Roles = "User")]
    public class SolutionController : Controller
    {
        private IUserManager _userManager;

        public SolutionController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();

            var dbModel = _userManager.Solutions
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.InsertTime)
                .ToList();

            var viewModel = new List<SendsSolutionViewModel>();

            dbModel.ForEach(x =>
            {
                SendsSolutionViewModel ssvm = new SendsSolutionViewModel
                {
                    SolutionId = x.Id,
                    TaskTitle = x.Task.Title,
                    TournamentTitle = x.Tournament.Title,
                    InsertTime = x.InsertTime,
                    MemUsage = x.MemoryUsage,
                    TimeUsage = x.ExecutedTime,
                    IsAccepted = x.Score == 0 ? false : true,
                    Lang = ((CompilerType)x.CompilerId).ToString(),
                    Error = x.Error
                };

                viewModel.Add(ssvm);
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
                _userManager.Dispose();
                _userManager = null;

                base.Dispose(disposing);
            }
        }

        #endregion Helpers
    }
}