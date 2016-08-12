using reCaptcha;
using SAP.BOL.Abstract;
using SAP.BOL.HelperClasses;
using SAP.Web.Infrastructrue.Server;
using SAP.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace SAP.Web.Controllers
{
    public class HomeController : Controller
    {
        private IContactManager _contactManager;
        private INewsManager _newsManager;

        public HomeController(IContactManager contactManager, INewsManager newsManager)
        {
            _contactManager = contactManager;
            _newsManager = newsManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About(int page = 1)
        {
            var viewModel = new InfoNewsViewModel();
            var dbModel = _newsManager.News.ToList();
            var listViewModel = new List<NewsViewModel>();

            viewModel.CurrentPage = page;
            viewModel.TotalPages = (int)Math.Ceiling((decimal)dbModel.Count / 5);

            dbModel = dbModel
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * 5)
                .Take(5)
                .ToList();

            dbModel.ForEach(x =>
            {
                var model = new NewsViewModel
                {
                    Title = x.Title,
                    Description = x.Description,
                    InsertTime = x.InsertTime
                };

                listViewModel.Add(model);
            });

            viewModel.News = listViewModel;

            return View(viewModel);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Formularz kontaktowy.";

            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactModel model, string ReturnUrl)
        {
            var privateKey = ServerDictionary.CaptchaPrivateKey;

            if (ReCaptcha.Validate(privateKey))
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
            else
            {
                TempData["Alert"] = SetAlert.Set("Musisz udowodnić że nie jesteś robotem poprzez captcha!", "Błąd", AlertType.Danger);
                return View();
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

            _newsManager.Dispose();
            _newsManager = null;

            base.Dispose(disposing);
        }

        #endregion Helpers
    }
}