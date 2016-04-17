using System.Web.Mvc;

namespace SAP.Web.Areas.Admin.Controllers
{
    public class ManageCompilersController : Controller
    {
        // GET: Admin/ManageCompilers
        public ActionResult Index()
        {
            return View();
        }

        #region Helpers

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                base.Dispose(disposing);
            }
        }

        #endregion Helpers
    }
}