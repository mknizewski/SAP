using SAP.BOL.HelperClasses;
using System.Web.Mvc;

namespace SAP.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            TempData["Alert"] = SetAlert.Set("Testowy message", "tag", AlertType.Success);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}