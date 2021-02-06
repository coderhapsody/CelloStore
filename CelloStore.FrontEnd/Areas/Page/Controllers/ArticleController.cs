using System;
using System.Web.Mvc;
using CelloStore.Data;
using CelloStore.FrontEnd.Attributes;
using CelloStore.FrontEnd.Base;
using CelloStore.Providers;
using CelloStore.ViewModels.Page.Article;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace CelloStore.FrontEnd.Areas.Page.Controllers
{
    public class ArticleController : AdminController
    {
        private readonly ArticleProvider articleProvider;
        public ArticleController(ArticleProvider articleProvider)
        {
            this.articleProvider = articleProvider;
        }

        [ImportModelStateFromTempData]
        public ActionResult Index()
        {
            var model = new IndexViewModel();
            return View(model);
        }

        [HttpPost]
        [ExportModelStateToTempData]
        public ActionResult Index(FormCollection form)
        {
            try
            {
                if(form["btnDelete"] != null)
                {
                    var articleValues = form.GetValues("chkDelete");
                    if(articleValues != null)
                    {
                        foreach(var articleValue in articleValues)
                        {
                            var articleId = Convert.ToInt32(articleValue);
                            articleProvider.DeleteArticle(articleId);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
                Logger.ErrorException(ex.Message, ex);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ImportModelStateFromTempData]
        public ActionResult Create()
        {
            var model = new CreateEditViewModel();
            model.Id = 0;
            return View(model);
        }

        [HttpPost]
        [ExportModelStateToTempData]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection form, CreateEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var article = new Article();
                    article = AutoMapper.Mapper.DynamicMap<Article>(model);                    
                    articleProvider.AddOrUpdateArticle(article);
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

        public ActionResult List([DataSourceRequest] DataSourceRequest request)
        {
            var query = articleProvider.List();
            return Json(query.ToDataSourceResult(request));
        }

        [HttpGet]
        [ImportModelStateFromTempData]
        public ActionResult Edit(int id)
        {
            var model = new CreateEditViewModel();
            var article = articleProvider.GetArticle(id);            
            model = AutoMapper.Mapper.DynamicMap<CreateEditViewModel>(article); 
            return View(model);
        }

        [HttpPost]
        [ExportModelStateToTempData]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection form, CreateEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var article = articleProvider.GetArticle(model.Id);
                    AutoMapper.Mapper.DynamicMap(model, article);
                    articleProvider.AddOrUpdateArticle(article);
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
    }
}