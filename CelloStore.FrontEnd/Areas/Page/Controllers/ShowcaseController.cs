using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using CelloStore.Data;
using CelloStore.FrontEnd.Attributes;
using CelloStore.FrontEnd.Base;
using CelloStore.Providers;
using CelloStore.ViewModels.Page.Showcase;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace CelloStore.FrontEnd.Areas.Page.Controllers
{
    public class ShowcaseController : AdminController
    {
        private readonly ShowcaseProvider showcaseProvider;
        private readonly ArticleProvider articleProvider;

        public ShowcaseController(ShowcaseProvider showcaseProvider, ArticleProvider articleProvider)
        {
            this.showcaseProvider = showcaseProvider;
            this.articleProvider = articleProvider;
        }

        public ActionResult GetArticles()
        {
            var articles = articleProvider.GetPublishedArticles();
            return Json(articles, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ImportModelStateFromTempData]
        public ActionResult Index(int? page, int? pageSize, string search, string sort)
        {
            var model = new IndexViewModel();
            return View(model);
        }

        public ActionResult List([DataSourceRequest] DataSourceRequest request)
        {
            var query = showcaseProvider.List();
            return Json(query.ToDataSourceResult(request));
        }

        [HttpPost]
        [ExportModelStateToTempData]
        public ActionResult Index(FormCollection form)
        {
            if (!String.IsNullOrEmpty(form["chkDelete"]))
            {
                try
                {
                    var rowIds = form.GetValues("chkDelete");
                    Array.ForEach(Array.ConvertAll(rowIds, Convert.ToInt32), showcaseProvider.DeleteShowcase);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ImportModelStateFromTempData]
        public ActionResult Create()
        {
            var model = new CreateEditViewModel();
            model.Id = 0;
            model.ExtLinkType = -1;
            return View(model);
        }

        [HttpPost]
        [ExportModelStateToTempData]
        public ActionResult Create(FormCollection form, CreateEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var showcase = new Showcase();
                    showcase.Title = model.Title;
                    showcase.Description = model.Description;
                    showcase.Url = model.Url;
                    showcase.ExtLinkType = model.ExtLinkType;
                    showcase.ArticleId = model.ArticleId;
                    showcase.FromDate = model.NoFromDate ? null : model.FromDate;
                    showcase.ToDate = model.NoToDate ? null : model.ToDate;
                    showcase.IsActive = model.IsActive;
                    showcase.ImagePath = model.ImagePath;
                    showcase.PriceImagePath = model.PriceImagePath;
                    showcaseProvider.AddOrUpdateShowcase(showcase);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Message);
                    Logger.ErrorException(ex.Message, ex);
                }
            }
            return RedirectToAction("Create");
        }

        [HttpGet]
        [ImportModelStateFromTempData]
        public ActionResult Edit(int id)
        {
            var model = new CreateEditViewModel();
            var showcase = showcaseProvider.GetShowcase(id);
            model.Id = showcase.Id;
            model.Title = showcase.Title;
            model.Description = showcase.Description;
            model.FromDate = showcase.FromDate;
            model.NoFromDate = !showcase.FromDate.HasValue;
            model.ToDate = showcase.ToDate;
            model.NoToDate = !showcase.ToDate.HasValue;
            model.Url = showcase.Url;
            model.ExtLinkType = showcase.ExtLinkType.GetValueOrDefault();
            model.ArticleId = showcase.ArticleId;
            model.PriceImagePath = showcase.PriceImagePath;
            model.ImagePath = showcase.ImagePath;
            model.IsActive = showcase.IsActive;
            model.First = showcase.First;
            return View(model);
        }

        [HttpPost]
        [ExportModelStateToTempData]
        public ActionResult Edit(FormCollection form, CreateEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var showcase = showcaseProvider.GetShowcase(model.Id);
                    showcase.Title = model.Title;
                    showcase.Description = model.Description;
                    showcase.Url = model.Url;
                    showcase.ExtLinkType = model.ExtLinkType;
                    showcase.ArticleId = model.ArticleId;
                    showcase.FromDate = model.NoFromDate ? null : model.FromDate;
                    showcase.ToDate = model.NoToDate ? null : model.ToDate;
                    showcase.IsActive = model.IsActive;
                    showcase.ImagePath = model.ImagePath;
                    showcase.PriceImagePath = model.PriceImagePath;
                    showcaseProvider.AddOrUpdateShowcase(showcase);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Message);
                    Logger.ErrorException(ex.Message, ex);
                }
            }

            return RedirectToAction("Edit");
        }

        public ActionResult Save(IEnumerable<HttpPostedFileBase> imageFiles)
        {
            if (imageFiles != null)
            {
                foreach (var file in imageFiles)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Media/Ads"), fileName);

                    file.SaveAs(physicalPath);
                }
            }

            return Content("");
        }

        public ActionResult SavePriceImages(IEnumerable<HttpPostedFileBase> priceFiles)
        {
            if (priceFiles != null)
            {
                foreach (var file in priceFiles)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Media/Ads"), fileName);

                    file.SaveAs(physicalPath);
                }
            }

            return Content("");
        }

        public ActionResult Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Media/Ads"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
    }
}