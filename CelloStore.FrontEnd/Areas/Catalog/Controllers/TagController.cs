using System;
using System.Web.Mvc;
using CelloStore.Data;
using CelloStore.FrontEnd.Attributes;
using CelloStore.FrontEnd.Base;
using CelloStore.Providers;
using CelloStore.ViewModels.Catalog.Tag;

namespace CelloStore.FrontEnd.Areas.Catalog.Controllers
{
    public class TagController : AdminController
    {
        private readonly TagProvider tagProvider;

        public TagController(TagProvider tagProvider)
        {
            this.tagProvider = tagProvider;
        }

        public ActionResult Index(int? page, int? pageSize, string search, string sort)
        {
            page = page ?? 1;
            pageSize = pageSize ?? 10;
            search = search ?? String.Empty;
            sort = sort ?? "ChangedWhen-desc";
            sort = String.Join(" ", sort.Split('-'));

            int rowCount = 0;
            var model = new IndexViewModel();
            model.List = tagProvider.ListTags(page.GetValueOrDefault(), pageSize.GetValueOrDefault(), search, sort, out rowCount);
            model.RowCount = rowCount;
            return View(model);
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
        public ActionResult Create(FormCollection form, CreateEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var tag = new Tag();
                    tag.Name = model.Name;
                    tagProvider.AddOrUpdateTag(tag);
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
            var tag = tagProvider.GetTag(id);
            model.Id = tag.Id;
            model.Name = tag.Name;
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
                    var area = tagProvider.GetTag(model.Id);
                    area.Name = model.Name;
                    tagProvider.AddOrUpdateTag(area);
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