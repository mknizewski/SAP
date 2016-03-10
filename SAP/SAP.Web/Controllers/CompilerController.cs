using SAP.BOL.Abstract;
using SAP.BOL.LogicClasses;
using SAP.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAP.Web.Controllers
{
    public class CompilerController : Controller
    {
        // GET: Compiler
        public ActionResult Index()
        {
            ViewBag.Compiler = GetCompilersList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Output(CompilerViewModel model)
        {
            ProgramManager _programManager = new ProgramManager();
            _programManager.Program = model.Program;
            _programManager.Language = (CompilerType)model.SelectedCompiler;
            _programManager.MaxTime = 15000;
            _programManager.InputDataType = InputDataType.Stream;
            _programManager.InputData = "1 4";
            _programManager.CompileAndExecute();

            ProgramViewModel modelToReturn = new ProgramViewModel
            {
                OutputData = _programManager.OutputData,
                ExecutedTime = _programManager.ExecutedTime,
                HasErrors = _programManager.HasError,
                ErrorData = _programManager.ErrorInfo,
                File = _programManager.CompiledFile,
                MemoryUsed = _programManager.MemoryUsed
            };

            _programManager.Dispose();
            return View(modelToReturn);
        }


        #region Helpers

        private List<SelectListItem> GetCompilersList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "C", Value = CompilerType.C.ToString() });
            list.Add(new SelectListItem { Text = "C++", Value = "1" });
            list.Add(new SelectListItem { Text = "Pascal", Value = "3" });

            return list;
        }

        #endregion
    }
}