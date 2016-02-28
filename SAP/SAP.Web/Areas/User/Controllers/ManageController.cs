using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SAP.BOL.Abstract;
using SAP.BOL.HelperClasses;
using SAP.DAL.DbContext;
using SAP.Web.Areas.User.Models;
using SAP.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SAP.Web.Areas.User.Controllers
{
    [Authorize(Roles = "User")]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _aspUserManager;
        private IUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager AspUserManager
        {
            get
            {
                return _aspUserManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _aspUserManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public ActionResult Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? SetAlert.Set("Twoje hasło zostało zmienione.", "Sukces", AlertType.Success)
                : message == ManageMessageId.ChangeSchoolDataSuccess ? SetAlert.Set("Dane szkoły zostały zaktualizowane.", "Sukces", AlertType.Success)
                : message == ManageMessageId.ChangeCounselorDataSuccess ? SetAlert.Set("Dane opiekuna zostały zaktualizowane.", "Sukces", AlertType.Success)
                : message == ManageMessageId.ChangeUserDataSuccess ? SetAlert.Set("Twoje dane zostały zaktualizowane.", "Sukces", AlertType.Success)
                : message == ManageMessageId.ChangeEmailSuccess ? SetAlert.Set("Twój email został zmieniony. Na twoją skrzynkę pocztową zostało wysłane nowe potwierdzenie adresu.", "Sukces", AlertType.Success)
                : message == ManageMessageId.Error ? SetAlert.Set("Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.", "Błąd", AlertType.Danger)
                : null;

            ViewBag.Email = AspUserManager.IsEmailConfirmed(User.Identity.GetUserId());
            ViewBag.Phone = AspUserManager.IsPhoneNumberConfirmed(User.Identity.GetUserId());

            return View();
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await AspUserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await AspUserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        public ActionResult ChangeUserData()
        {
            var dbModel = AspUserManager.FindById(User.Identity.GetUserId());
            
            UserDataViewModel viewModel = new UserDataViewModel
            {
                Name = dbModel.FirstName,
                Surname = dbModel.LastName,
                UserCity = dbModel.City,
                UserHouseNumber = dbModel.HouseNumber,
                UserPhone = dbModel.PhoneNumber,
                UserPostalCode = dbModel.PostalCode,
                UserStreet = dbModel.Street
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeUserData(UserDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = AspUserManager.FindById(User.Identity.GetUserId());

                user.City = model.UserCity;
                user.HouseNumber = model.UserHouseNumber;
                user.PostalCode = model.UserPostalCode;
                user.PhoneNumber = model.UserPhone;
                user.Street = model.UserStreet;
                user.FirstName = model.Name;
                user.LastName = model.Surname;

               var result = await AspUserManager.UpdateAsync(user);

                if (result.Succeeded)
                    return RedirectToAction("Index", new { Message = ManageMessageId.ChangeUserDataSuccess });
                else
                    return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            else
                return View(model);
        }

        public ActionResult ChangeSchool()
        {
            var dbModel = _userManager.GetUserSchoolById(User.Identity.GetUserId()); 

            UserSchoolViewModel viewModel = new UserSchoolViewModel
            {
                SchoolName = dbModel.Name,
                SchoolCity = dbModel.City,
                SchoolClass = dbModel.Class,
                SchoolHouseNumber = dbModel.HouseNumber,
                SchoolPhone = dbModel.Phone,
                SchoolPostalCode = dbModel.PostalCode,
                SchoolStreet = dbModel.Street
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeSchool(UserSchoolViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool result = _userManager.ChangeUserSchool(User.Identity.GetUserId(), model.SchoolName, model.SchoolClass, model.SchoolCity,
                    model.SchoolHouseNumber, model.SchoolPostalCode, model.SchoolStreet, model.SchoolPhone);

                if (result)
                    return RedirectToAction("Index", new { Message = ManageMessageId.ChangeSchoolDataSuccess });
                else
                    return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            else
                return View(model);
        }

        public ActionResult ChangeCounselor()
        {
            var dbModel = _userManager.GetUserCounselorById(User.Identity.GetUserId());

            UserCounselorViewModel viewModel = new UserCounselorViewModel
            {
                CounselorFirstName = dbModel.FirstName,
                CounselorLastName = dbModel.LastName
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeCounselor(UserCounselorViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool result = _userManager.ChangeUserCounselor(User.Identity.GetUserId(), model.CounselorFirstName, model.CounselorLastName);

                if (result)
                    return RedirectToAction("Index", new { Message = ManageMessageId.ChangeCounselorDataSuccess });
                else
                    return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            else
                return View(model);
        }

        public ActionResult ChangeEmail()
        {
            TempData["Alert"] = SetAlert.Set("Zmieniając email będziesz musiał logować się przy pomocy nowego adresu.", "Uwaga", AlertType.Warning);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeEmail(UserChangeEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = AspUserManager.FindById(User.Identity.GetUserId());

                user.Email = model.Email;
                user.EmailConfirmed = false;
                user.UserName = model.Email;

                var result = await AspUserManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    //TODO: Wysłać email z potwierdzeniem

                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }

                    return RedirectToAction("Index", new { Message = ManageMessageId.ChangeEmailSuccess });
                }
                else
                    return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            else
                return View();
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await AspUserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (AspUserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await AspUserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await AspUserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await AspUserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await AspUserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await AspUserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await AspUserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await AspUserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await AspUserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // GET: /Manage/RemovePhoneNumber
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await AspUserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await AspUserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
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
                var user = await AspUserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            else
            {
                TempData["Alert"] = SetAlert.Set("Wprowadzone hasło jest niepoprawne!", "Błąd", AlertType.Danger);
                return View(model);
            }
            
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await AspUserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await AspUserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await AspUserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await AspUserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await AspUserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                AspUserManager.Dispose();
                AspUserManager = null;
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = AspUserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = AspUserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            ChangeSchoolDataSuccess,
            ChangeUserDataSuccess,
            ChangeEmailSuccess,
            ChangeCounselorDataSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion Helpers
    }
}