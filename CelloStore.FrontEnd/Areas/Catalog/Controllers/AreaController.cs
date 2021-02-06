using System;
using System.Web.Mvc;
using CelloStore.Data;
using CelloStore.FrontEnd.Attributes;
using CelloStore.FrontEnd.Base;
using CelloStore.Providers;
using CelloStore.ViewModels.Catalog.Area;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace CelloStore.FrontEnd.Areas.Catalog.Controllers
{
    public class AreaController : AdminController
    {
        private readonly AreaProvider areaProvider;
            
        public AreaController(AreaProvider areaProvider)
        {
            this.areaProvider = areaProvider;
        }

        public ActionResult Index()
        {            
            var model = new IndexViewModel();            
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            try
            {
                if(form["btnDelete"] != null)
                {
                    var areaValues = form.GetValues("chkDelete");
                    if(areaValues != null)
                    {
                        foreach(var areaValue in areaValues)
                        {
                            var areaId = Convert.ToInt32(areaValue);
                            areaProvider.DeleteArea(areaId);
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

        public ActionResult List([DataSourceRequest] DataSourceRequest request)
        {
            var query = areaProvider.List();
            return Json(query.ToDataSourceResult(request));
        }

        [HttpGet]
        [ImportModelStateFromTempData]
        public ActionResult Create()
        {
            var model = new CreateEditViewModel();
            model.Id = 0;
            model.IsActive = true;
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
                    var area = new Area();
                    area.Name = model.Name;
                    area.IsActive = model.IsActive;
                    areaProvider.AddOrUpdateArea(area);

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
            var area = areaProvider.GetArea(id);
            model.Id = area.Id;
            model.Name = area.Name;
            model.IsActive = area.IsActive;
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
                    var area = areaProvider.GetArea(model.Id);
                    area.Name = model.Name;
                    area.IsActive = model.IsActive;
                    areaProvider.AddOrUpdateArea(area);                    
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