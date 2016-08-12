using Resources;
using SAP.BOL.Abstract;
using SAP.BOL.HelperClasses;
using SAP.BOL.LogicClasses;
using SAP.Workers;
using System;
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

            _compilerResource = new ResourceManager(typeof(CompilersResource));
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetInfo()
        {
            try
            {
                var sandboxService = SandboxService.Create(RequestType.Info);
                string responseFromServer = sandboxService.MakeRequest();

                TempData["response"] = responseFromServer;
            }
            catch (Exception ex)
            {
                TempData["Alert"] = SetAlert.Set("Wystąpił błąd: " + ex.Message, "Błąd komunikacji", AlertType.Danger);
            }

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