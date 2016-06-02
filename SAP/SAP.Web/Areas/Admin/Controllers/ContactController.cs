using SAP.BOL.Abstract;
using SAP.BOL.HelperClasses;
using SAP.Web.Areas.Admin.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SAP.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ContactController : Controller
    {
        private IContactManager _contactManager;

        public ContactController(IContactManager contactManager)
        {
            _contactManager = contactManager;
        }

        public ActionResult Index()
        {
            var viewModel = new List<ContactViewModel>();
            var dbModel = _contactManager.Contacts
                .ToList();

            dbModel.ForEach(x =>
            {
                var model = new ContactViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Surname = x.Surname,
                    Email = x.Email
                };

                viewModel.Add(model);
            });

            return View(viewModel);
        }

        public JsonResult Read(int messageId)
        {
            string message = _contactManager.Contacts
                .Where(x => x.Id == messageId)
                .Select(x => x.Content)
                .FirstOrDefault();

            return Json(message);
        }

        public ActionResult Delete(int messageId)
        {
            bool result = _contactManager.Delete(messageId);

            if (result)
                TempData["Alert"] = SetAlert.Set("Poprawnie usunięto wiadomość!", "Sukces", AlertType.Success);
            else
                TempData["Alert"] = SetAlert.Set("Wystąpił błąd", "Błąd", AlertType.Danger);

            return RedirectToAction("Index");
        }

        #region Helpers

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _contactManager.Dispose();
                _contactManager = null;

                base.Dispose(disposing);
            }
        }

        #endregion Helpers
    }
}