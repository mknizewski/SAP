using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SAP.BOL.Abstract;
using SAP.BOL.HelperClasses;
using SAP.DAL.DbContext;
using SAP.DAL.Tables;
using SAP.Web.Areas.Admin.Models;
using SAP.Web.HTMLHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SAP.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
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

        private IUserManager _userManager;

        public ManageUserController(IUserManager userManager)
        {
            _userManager = userManager;
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
                return RedirectToAction("Index", new { Message = Message.ChagnePasswordSuccess });
            }
            else
            {
                TempData["Alert"] = SetAlert.Set("Wprowadzone hasło jest niepoprawne!", "Błąd", AlertType.Danger);
                return View(model);
            }
        }

        public ActionResult ManageAccounts()
        {
            return View();
        }

        
        public ActionResult AddAdministrator()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAdministrator(AddAdminViewModel viewModel)
        {
            ApplicationUser user = new ApplicationUser
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                UserName = viewModel.Email,
                Email = viewModel.Email,
                EmailConfirmed = true
            };

            AspUserManager.Create(user, viewModel.Password);
            AspUserManager.AddToRole(user.Id, "Admin");

            TempData["Alert"] = SetAlert.Set("Konto poprawnie stworzone!", "Sukces", AlertType.Success);
            return RedirectToAction("ManageAccounts");
        }

        public ActionResult GetUserData(UserTypeToReturn typeToReturn)
        {
            List<UsersViewModel> viewModel = new List<UsersViewModel>();
            string userId = User.Identity.GetUserId();

            switch (typeToReturn)
            {
                case UserTypeToReturn.Administrator:
                    AspUserManager.Users
                       .Where(x => x.Id != userId)
                       .ForEach(x =>
                       {
                           bool isAdmin = AspUserManager.IsInRole(x.Id, "Admin");
                           if (isAdmin)
                           {
                               viewModel.Add(new UsersViewModel
                               {
                                   Id = x.Id,
                                   FirstName = x.FirstName,
                                   LastName = x.LastName,
                                   Email = x.Email,
                                   Role = "Administrator",
                                   IsLocked = AspUserManager.IsLockedOut(x.Id)
                               });
                           }
                       });
                    break;

                case UserTypeToReturn.User:
                    AspUserManager.Users
                        .Where(x => x.Id != userId)
                        .ForEach(x =>
                        {
                            bool isUser = AspUserManager.IsInRole(x.Id, "User");
                            if (isUser)
                            {
                                viewModel.Add(new UsersViewModel
                                {
                                    Id = x.Id,
                                    FirstName = x.FirstName,
                                    LastName = x.LastName,
                                    Email = x.Email,
                                    Role = "Użytkownik",
                                    IsLocked = AspUserManager.IsLockedOut(x.Id)
                                });
                            }
                        });
                    break;

                case UserTypeToReturn.All:
                    AspUserManager.Users
                        .Where(x => x.Id != userId)
                        .ForEach(x => viewModel.Add(new UsersViewModel
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            Email = x.Email,
                            Role = AspUserManager.GetRoles(x.Id).FirstOrDefault(),
                            IsLocked = AspUserManager.IsLockedOut(x.Id)
                        }));
                    break;

                case UserTypeToReturn.Banned:
                    AspUserManager.Users
                        .Where(x => x.Id != userId)
                        .ForEach(x =>
                        {
                            bool result = AspUserManager.IsLockedOut(x.Id);
                            if (result)
                            {
                                viewModel.Add(new UsersViewModel
                                {
                                    Id = x.Id,
                                    FirstName = x.FirstName,
                                    LastName = x.LastName,
                                    Email = x.Email,
                                    Role = AspUserManager.GetRoles(x.Id).FirstOrDefault(),
                                    IsLocked = true
                                });
                            }
                        });
                    break;

                case UserTypeToReturn.NotBanned:
                    AspUserManager.Users
                        .Where(x => x.Id != userId)
                        .ForEach(x =>
                        {
                            bool result = AspUserManager.IsLockedOut(x.Id);
                            if (!result)
                            {
                                viewModel.Add(new UsersViewModel
                                {
                                    Id = x.Id,
                                    FirstName = x.FirstName,
                                    LastName = x.LastName,
                                    Email = x.Email,
                                    Role = AspUserManager.GetRoles(x.Id).FirstOrDefault(),
                                    IsLocked = false
                                });
                            }
                        });
                    break;
            }

            return PartialView("~/Areas/Admin/Views/Shared/UserList.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Ban(string name, string banTime, string banDate, bool isPernament)
        {
            MvcHtmlString alert = null;

            if (isPernament)
            {
                var user = AspUserManager.FindByEmail(name);
                AspUserManager.SetLockoutEndDate(user.Id, DateTimeOffset.MaxValue);

                alert = Alert.GetAlert(SetAlert.Set("Pernamentnie zbanowano " + name, "Sukces", AlertType.Success));
            }
            else
            {
                DateTime endDate;
                string dateAndTime = banDate + " " + banTime;
                bool parseResult = DateTime.TryParse(dateAndTime, out endDate);

                if (parseResult)
                {
                    if (endDate >= DateTime.Now)
                    {
                        var user = AspUserManager.FindByEmail(name);
                        AspUserManager.SetLockoutEndDate(user.Id, endDate);

                        alert = Alert.GetAlert(SetAlert.Set("Poprawnie zablokowano " + name, "Sukces", AlertType.Success));
                    }
                    else
                        alert = Alert.GetAlert(SetAlert.Set("Musisz podać datę większą od dnia dzisiejszego", "Błąd", AlertType.Danger));
                }
                else
                    alert = Alert.GetAlert(SetAlert.Set("Data lub godzina została podana błędnie. Schemat: dd-mm-rrrr hh:ss", "Błąd", AlertType.Danger));
            }

            return Json(new { Message = alert.ToHtmlString() });
        }

        [HttpPost]
        public ActionResult UnBan(string name)
        {
            var userId = AspUserManager.FindByEmail(name).Id;
            AspUserManager.SetLockoutEndDate(userId, DateTimeOffset.MinValue);

            MvcHtmlString returnValue = Alert.GetAlert(SetAlert.Set("Poprawnie odblokowano " + name, "Sukces", AlertType.Success));

            return Json(new { Message = returnValue.ToHtmlString() });
        }

        [HttpPost]
        public ActionResult GetDetail(string name)
        {
            var user = AspUserManager.FindByEmail(name);
            var userData = _userManager.GetUserDataById(user.Id);

            var jsonData = new
            {
                UserFirstName = user.FirstName,
                UserLastName = user.LastName,
                UserEmail = user.Email,
                UserCity = user.City,
                UserStreet = user.Street + " " + user.HouseNumber,
                UserPostalCode = user.PostalCode,
                UserPhone = user.PhoneNumber,

                CounselorData = userData.Counselor != null ? userData.Counselor.FirstName + " " + userData.Counselor.LastName : "Brak wpisu",

                SchoolName = userData.School.Name,
                SchoolClass = userData.School.Class,
                SchoolCity = userData.School.City,
                SchoolStreet = userData.School.Street + " " + userData.School.HouseNumber,
                SchoolPostalCode = userData.School.PostalCode,
                SchoolPhone = userData.School.Phone,

                UserBan = user.LockoutEndDateUtc.HasValue ? user.LockoutEndDateUtc.Value.ToString("HH:mm:ss, dd.MM.yyyy") : "Nie zablokowany"
            };

            return Json(jsonData);
        }

        public ActionResult SendMessageToUser(string name, string title, string desc)
        {
            var user = AspUserManager.FindByName(name);
            bool result = _userManager.SendMessage(user.Id, title, desc);
            MvcHtmlString jsonData = null;

            if (result)
                jsonData = Alert.GetAlert(SetAlert.Set("Poprawnie wysłano wiadomość do " + name, "Sukces", AlertType.Success));
            else
                jsonData = Alert.GetAlert(SetAlert.Set("Wystąpił bład. Spróbuj ponownie później.", "Błąd", AlertType.Danger));

            return Json(new { Message = jsonData.ToHtmlString() });
        }

        #region Helpers

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                AspUserManager.Dispose();
                SignInManager.Dispose();
                _userManager.Dispose();

                base.Dispose(disposing);
            }
        }

        #endregion Helpers
    }

    public enum UserTypeToReturn
    {
        Administrator, User, All, NotBanned, Banned
    }
}