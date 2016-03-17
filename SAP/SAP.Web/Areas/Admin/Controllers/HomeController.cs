using Microsoft.AspNet.Identity.Owin;
using SAP.BOL.HelperClasses;
using SAP.DAL.DbContext;
using SAP.Web.Areas.Admin.Models;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SAP.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        public ApplicationUserManager AspUserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        // GET: Admin/Home
        public async Task<ActionResult> Index(Message? message)
        {
            ViewBag.Message = message == Message.ChangeDataSuccess ? SetAlert.Set("Dane zostały poprawnie zmienione!", "Sukces", AlertType.Success)
                : message == Message.ChagnePasswordSuccess ? SetAlert.Set("Hasło zostało poprawnie zmienione!", "Sukces", AlertType.Success)
                : message == Message.Error ? SetAlert.Set("Wystąpił błąd, spróbuj ponownie poźniej", "Uwaga", AlertType.Danger)
                : null;

            var adminData = await AspUserManager.FindByNameAsync(User.Identity.Name);

            AdminDataViewModel viewModel = new AdminDataViewModel
            {
                FirstName = adminData.FirstName,
                LastName = adminData.LastName,
                Email = adminData.Email,
                Role = HttpContext.User.IsInRole("Root") ? "Root (super admin)" : "Administrator"
            };

            return View(viewModel);
        }

        #region Helpers

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                AspUserManager.Dispose();

                base.Dispose(disposing);
            }
        }

        #endregion Helpers
    }

    public enum Message
    {
        ChangeDataSuccess,
        ChagnePasswordSuccess,
        Error
    }
}