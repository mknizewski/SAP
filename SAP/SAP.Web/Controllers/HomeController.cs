using SAP.BOL.Abstract;
using SAP.BOL.HelperClasses;
using SAP.Web.Models;
using System;
using System.Web.Mvc;

namespace SAP.Web.Controllers
{
    public class HomeController : Controller
    {
        private IContact _contact;

        public HomeController(IContact contact)
        {
            _contact = contact;
        }

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
            if (_contact.AddNewContact(model.Name, model.Surname, model.Email, model.Message))
            {
                TempData["Alert"] = SetAlert.Set("Dziękujemy za wiadomość!", "Sukces", AlertType.Success);

                return RedirectToLocal(ReturnUrl);
            }
            else
            {
                TempData["Alert"] = SetAlert.Set("Wystąpił błąd, prosimy spróbwać ponownie później.", "Błąd", AlertType.Danger);

                return RedirectToAction("Contact");
            }
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

            _contact = null;
        }

        #endregion Helpers
    }
}