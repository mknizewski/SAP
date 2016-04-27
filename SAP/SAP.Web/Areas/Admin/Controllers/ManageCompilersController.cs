using SAP.BOL.Abstract;
using SAP.BOL.HelperClasses;
using SAP.BOL.LogicClasses;
using SAP.Web.Areas.Admin.Models;
using System.Web.Mvc;

namespace SAP.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageCompilersController : Controller
    {
        private ICompilersManager _compilerManager;

        public ManageCompilersController(ICompilersManager compilerManager)
        {
            _compilerManager = compilerManager;
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

                base.Dispose(disposing);
            }
        }

        #endregion Helpers
    }
}