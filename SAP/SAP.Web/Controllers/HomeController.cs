using SAP.BOL.HelperClasses;
using SAP.Web.Models;
using System;
using System.Web.Mvc;

namespace SAP.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string browser = Request.Browser.Browser;

            //TODO: Postarać się o lepszą implementację -- odzielić do BO!
            if (String.Equals(browser, "InternetExplorer"))
                TempData["Alert"] = SetAlert.Set("Przeglądarka IE oraz Edge nie jest wspierana!", "Uwaga", AlertType.Warning);

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Formularz kontaktowy.";

            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactModel model, string ReturnUrl)
        {
            TempData["Alert"] = SetAlert.Set("Dziękujemy za wiadomość!", "Sukces", AlertType.Success);

            //TODO: Dodać obsługę formularza kontaktowego

            return RedirectToLocal(ReturnUrl);
        }

        #region Helpers

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #endregion Helpers
    }
}