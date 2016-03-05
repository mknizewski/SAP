using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SAP.BOL.HelperClasses;
using SAP.DAL.DbContext;
using SAP.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SAP.Web.Areas.Admin.Controllers
{
    public class ManageUserController : Controller
    {
        public ApplicationUserManager AspUserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
        }

        public async Task<ActionResult> ChangeData()
        {
            var userData = await AspUserManager.FindByNameAsync(User.Identity.Name);
            var viewModel = new AdminDataViewModel
            {
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                Email = userData.Email
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeData(AdminDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var adminData = await AspUserManager.FindByIdAsync(User.Identity.GetUserId());

                adminData.FirstName = model.FirstName;
                adminData.LastName = model.LastName;
                adminData.Email = model.Email;
                adminData.UserName = model.Email;

                var result = await AspUserManager.UpdateAsync(adminData);

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(adminData, isPersistent: false, rememberBrowser: false);
                    return RedirectToAction("Index", "Home", new { Message = Message.ChangeDataSuccess });
                }
                else
                    return RedirectToAction("Index", "Home", new { Message = Message.Error });
            }
            else
                return RedirectToAction("Index", "Home", new { Message = Message.Error });
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await AspUserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await AspUserManager.FindByNameAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = Message.ChagnePasswordSuccess});
            }
            else
            {
                TempData["Alert"] = SetAlert.Set("Wprowadzone hasło jest niepoprawne!", "Błąd", AlertType.Danger);
                return View(model);
            }

        }

        #region Helpers
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                AspUserManager.Dispose();
                SignInManager.Dispose();

                base.Dispose(disposing);
            }
        }
        #endregion
    }
}