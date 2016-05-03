using Microsoft.AspNet.Identity;
using SAP.BOL.Abstract;
using SAP.BOL.HelperClasses;
using SAP.Web.Areas.User.Models;
using SAP.Web.HTMLHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SAP.Web.Areas.User.Controllers
{
    [Authorize(Roles = "User")]
    public class MessagesController : Controller
    {
        private IUserManager _userManager;

        public MessagesController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var viewModel = new List<MessagesViewModel>();
            var messages = _userManager.Messages
                .Where(x => x.UserId == userId)
                .ToList();

            messages.ForEach(x =>
            {
                var model = new MessagesViewModel
                {
                    Id = x.Id,
                    Content = x.Description,
                    Title = x.Title,
                    InsertTime = x.SendTime
                };

                viewModel.Add(model);
            });

            return View(viewModel);
        }

        public JsonResult Delete(int messageId)
        {
            bool result = _userManager.DeleteMessage(messageId);
            MvcHtmlString alert;

            if (result)
                alert = Alert.GetAlert(SetAlert.Set("Poprawnie usunięto wiadomość!", "Sukces", AlertType.Success));
            else
                alert = Alert.GetAlert(SetAlert.Set("Wystąpił błąd!", "Błąd", AlertType.Danger));

            return Json(alert.ToHtmlString());
        }

        #region Helpers
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userManager.Dispose();
                _userManager = null;

                base.Dispose(disposing);
            }
        }
        #endregion
    }
}