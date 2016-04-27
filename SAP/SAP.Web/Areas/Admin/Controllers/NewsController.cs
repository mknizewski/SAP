using SAP.BOL.Abstract;
using SAP.BOL.HelperClasses;
using SAP.Web.Areas.Admin.Models;
using SAP.Web.HTMLHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SAP.Web.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        private INewsManager _newsManager;

        public NewsController(INewsManager newsManager)
        {
            _newsManager = newsManager;
        }

        public ActionResult Index(int page = 1)
        {
            var viewModel = new InfoNewsViewModel();
            var dbModel = _newsManager.News.ToList();
            int totalPages = dbModel.Count;

            dbModel = dbModel
                .OrderBy(x => x.Id)
                .Skip((page - 1) * 5)
                .Take(5)
                .ToList();

            viewModel.CurrentPage = page;
            viewModel.TotalPages = (int)Math.Ceiling((decimal)(totalPages) / 5);

            var listViewModel = new List<NewsViewModel>();

            dbModel.ForEach(x =>
            {
                var model = new NewsViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    InsertTime = x.InsertTime
                };

                listViewModel.Add(model);
            });

            viewModel.News = listViewModel;

            return View(viewModel);
        }

        public JsonResult AddNews(string title, string description)
        {
            bool result = _newsManager.AddNews(title, description);
            MvcHtmlString jsonResult;

            if (result)
                jsonResult = Alert.GetAlert(SetAlert.Set("Poprawnie dodano news!", "Sukcess", AlertType.Success));
            else
                jsonResult = Alert.GetAlert(SetAlert.Set("Wystąpił błąd, spróbuj ponownie później!", "Błąd", AlertType.Danger));

            return Json(jsonResult.ToHtmlString());
        }

        public JsonResult EditNews(int id, string desc)
        {
            bool result = _newsManager.EditNews(id, desc);
            MvcHtmlString jsonResult;

            if (result)
                jsonResult = Alert.GetAlert(SetAlert.Set("Poprawnie zaktualizowano news!", "Sukcess", AlertType.Success));
            else
                jsonResult = Alert.GetAlert(SetAlert.Set("Wystąpił błąd, spróbuj ponownie później!", "Błąd", AlertType.Danger));

            return Json(jsonResult.ToHtmlString());
        }

        public JsonResult DeleteNews(int id)
        {
            bool result = _newsManager.DeleteNews(id);
            MvcHtmlString jsonResult;

            if (result)
                jsonResult = Alert.GetAlert(SetAlert.Set("Poprawnie usunięto news!", "Sukcess", AlertType.Success));
            else
                jsonResult = Alert.GetAlert(SetAlert.Set("Wystąpił błąd, spróbuj ponownie później!", "Błąd", AlertType.Danger));

            return Json(jsonResult.ToHtmlString());
        }

        #region Helpers

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _newsManager.Dispose();
                _newsManager = null;

                base.Dispose(disposing);
            }
        }

        #endregion Helpers
    }
}