using SAP.BOL.Abstract;
using SAP.BOL.HelperClasses;
using SAP.Web.Models;
using System.Web.Mvc;

namespace SAP.Web.Controllers
{
    public class HomeController : Controller
    {
        private IContactManager _contactManager;

        public HomeController(IContactManager contactManager)
        {
            _contactManager = contactManager;
        }

        public ActionResult Index()
        {
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
            bool result = _contactManager.AddNewContact(model.Name, model.Surname, model.Email, model.Message);

            if (result)
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
            _contactManager.Dispose();
            _contactManager = null;

            base.Dispose(disposing);
        }

        #endregion Helpers
    }
}