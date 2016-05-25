using SAP.BOL.Abstract;
using SAP.BOL.HelperClasses;
using SAP.BOL.LogicClasses;
using SAP.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Web.Mvc;

namespace SAP.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageCompilersController : Controller
    {
        private ICompilersManager _compilerManager;
        private ResourceManager _compilerResource;
        private IProgramManager _programManager;

        public ManageCompilersController(ICompilersManager compilerManager, IProgramManager programManager)
        {
            _compilerManager = compilerManager;
            _programManager = programManager;

            Assembly assembly = Assembly.Load("App_GlobalResources");
            _compilerResource = new ResourceManager("Resources.CompilersResource", assembly);
        }

        public ActionResult Index()
        {
            CompilerViewModel viewModel = new CompilerViewModel();

            viewModel.CSystemId = (int)CompilerType.C;
            viewModel.CppSystemId = (int)CompilerType.Cpp;
            viewModel.JavaSystemId = (int)CompilerType.Java;
            viewModel.PascalSystemId = (int)CompilerType.Pascal;

            viewModel.CPath = CompilerInfo.CPath;
            viewModel.CppPath = CompilerInfo.CppPath;
            viewModel.JavaPath = CompilerInfo.JavaPath;
            viewModel.PascalPath = CompilerInfo.PascalPath;

            return View(viewModel);
        }

        public ActionResult Test()
        {
            List<TestCompilerViewModel> viewModel = new List<TestCompilerViewModel>();
            string cHello = _compilerResource.GetString("CHelloWorld");
            string cppHello = _compilerResource.GetString("CppHelloWorld");
            string javaHello = _compilerResource.GetString("JavaHelloWorld");
            string pascalHello = _compilerResource.GetString("PascalHelloWorld");

            string output = String.Empty;
            bool hasError;
            string errorString = String.Empty;
            string language;

            _programManager.MaxTime = 5;
            _programManager.InputDataType = InputDataType.None;
            _programManager.MaxMemory = 5;

            //C - Test
            _programManager.Language = CompilerType.C;
            _programManager.Program = cHello;
            _programManager.CompileAndExecute();

            hasError = _programManager.HasError;
            language = CompilerType.C.ToString();
            errorString = _programManager.ErrorInfo;
            output = _programManager.OutputData;
            viewModel.Add(TestCompilerViewModel.Inicialize(output, hasError, errorString, language));

            //Cpp - Test
            _programManager.Language = CompilerType.Cpp;
            _programManager.Program = cppHello;
            _programManager.CompileAndExecute();

            hasError = _programManager.HasError;
            language = CompilerType.Cpp.ToString();
            errorString = _programManager.ErrorInfo;
            output = _programManager.OutputData;
            viewModel.Add(TestCompilerViewModel.Inicialize(output, hasError, errorString, language));

            //Java - Test
            _programManager.Language = CompilerType.Java;
            _programManager.JavaMainClass = "Hello";
            _programManager.Program = javaHello;
            _programManager.CompileAndExecute();

            hasError = _programManager.HasError;
            language = CompilerType.Java.ToString();
            errorString = _programManager.ErrorInfo;
            output = _programManager.OutputData;
            viewModel.Add(TestCompilerViewModel.Inicialize(output, hasError, errorString, language));

            //Pascal - Test
            _programManager.Language = CompilerType.Pascal;
            _programManager.Program = pascalHello;
            _programManager.CompileAndExecute();

            hasError = _programManager.HasError;
            language = CompilerType.Pascal.ToString();
            errorString = _programManager.ErrorInfo;
            output = _programManager.OutputData;
            viewModel.Add(TestCompilerViewModel.Inicialize(output, hasError, errorString, language));

            return View(viewModel);
        }

        public ActionResult ChangePath(int systemId, string path)
        {
            bool result = _compilerManager.EditPath(systemId, path);

            if (result)
                TempData["Alert"] = SetAlert.Set("Poprawnie zmieniono scieżkę!", "Sukces", AlertType.Success);
            else
                TempData["Alert"] = SetAlert.Set("Wystąpił błąd!", "Błąd", AlertType.Danger);

            ServerConfig.CompilersSetup();
            return RedirectToAction("Index");
        }

        #region Helpers

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _compilerManager.Dispose();
                _compilerManager = null;

                _programManager.Dispose();
                _programManager = null;

                _compilerResource = null;

                base.Dispose(disposing);
            }
        }

        #endregion Helpers
    }
}