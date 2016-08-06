using SAP.BOL.Abstract;
using SAP.BOL.HelperClasses;
using SAP.BOL.LogicClasses;
using SAP.Web.Areas.Admin.Models;
using SAP.Workers;
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

        public ManageCompilersController(ICompilersManager compilerManager)
        {
            _compilerManager = compilerManager;

            Assembly assembly = Assembly.Load("App_GlobalResources");
            _compilerResource = new ResourceManager("Resources.CompilersResource", assembly);
        }

        public ActionResult Index()
        {
            

            return View();
        }

        public ActionResult GetInfo()
        {
            var sandboxService = SandboxService.Create(RequestType.Info);
            string responseFromServer = sandboxService.MakeRequest();

            TempData["response"] = responseFromServer;

            return View();
        }

        public ActionResult ChangePath(int systemId, string path, string arguments)
        {
            bool resultPath = _compilerManager.EditPath(systemId, path);
            bool resultArg = _compilerManager.EditArguments(systemId, arguments);
            
            if (resultPath && resultArg)
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

                _compilerResource = null;

                base.Dispose(disposing);
            }
        }

        #endregion Helpers
    }
}