using System;
using System.Web.Mvc;
using CelloStore.Data;
using CelloStore.FrontEnd.Attributes;
using CelloStore.FrontEnd.Base;
using CelloStore.Providers;
using CelloStore.ViewModels.Catalog.Category;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace CelloStore.FrontEnd.Areas.Catalog.Controllers
{
    public class CategoryController : AdminController
    {
        private readonly CategoryProvider categoryProvider;

        public CategoryController(CategoryProvider categoryProvider)
        {
            this.categoryProvider = categoryProvider;
        }

        public ActionResult Index()
        {            
            var model = new IndexViewModel();            
            return View(model);
        }

        public ActionResult List([DataSourceRequest] DataSourceRequest request)
        {
            var query = categoryProvider.List();
            return Json(query.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            try
            {
                if(form["btnDelete"] != null)
                {
                    var categoryValues = form.GetValues("chkDelete");
                    if(categoryValues != null)
                    {
                        foreach(var categoryValue in categoryValues)
                        {
                            var categoryId = Convert.ToInt32(categoryValue);
                            categoryProvider.DeleteCategory(categoryId);
                        }
                    }
                }
            }
            catch (Exception ex)
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
            model.IsActive = true;
            model.ParentCategories = categoryProvider.GetParentCategories();
            return View(model);
        }

        [HttpPost]
        [ExportModelStateToTempData]
        public ActionResult Create(FormCollection form, CreateEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var category = new Category();
                    category.Name = model.Name;
                    category.ParentCategoryId = model.ParentCategoryId;
                    category.IsActive = model.IsActive;
                    categoryProvider.AddOrUpdateCategory(category);
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
            var category = categoryProvider.GetCategory(id);
            model.Id = category.Id;
            model.Name = category.Name;
            model.IsActive = category.IsActive;
            model.ParentCategoryId = category.ParentCategoryId;
            model.ParentCategories = categoryProvider.GetParentCategories();
            return View(model);
        }

        [HttpPost]
        [ExportModelStateToTempData]
        public ActionResult Edit(FormCollection form, CreateEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var category = categoryProvider.GetCategory(model.Id);
                    category.Name = model.Name;
                    category.ParentCategoryId = model.ParentCategoryId;
                    category.IsActive = model.IsActive;
                    categoryProvider.AddOrUpdateCategory(category);
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

        public string GetParentCategoryName(int? id)
        {
            if(id.HasValue)
            {
                var category = categoryProvider.GetCategory(id.Value);
                return category.Name;
            }
            return String.Empty;
        }
    }
}