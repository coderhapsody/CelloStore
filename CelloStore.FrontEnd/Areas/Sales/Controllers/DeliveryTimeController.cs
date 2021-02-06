using System;
using System.Web.Mvc;
using AutoMapper;
using CelloStore.Data;
using CelloStore.FrontEnd.Attributes;
using CelloStore.FrontEnd.Base;
using CelloStore.Providers;
using CelloStore.ViewModels.Sales.DeliveryTime;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace CelloStore.FrontEnd.Areas.Sales.Controllers
{
    public class DeliveryTimeController : AdminController
    {
        private readonly DeliveryTimeProvider deliveryTimeProvider;

        public DeliveryTimeController(DeliveryTimeProvider deliveryTimeProvider)
        {
            this.deliveryTimeProvider = deliveryTimeProvider;
        }

        public ActionResult Index()
        {
            var model = new IndexViewModel();            
            return View(model);
        }

        public ActionResult List([DataSourceRequest] DataSourceRequest request)
        {
            var query = deliveryTimeProvider.List();
            return Json(query.ToDataSourceResult(request));
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
                    var deliveryTime = new DeliveryTime();
                    Mapper.DynamicMap(model, deliveryTime);
                    deliveryTimeProvider.UpdateDeliveryTime(deliveryTime);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
            return RedirectToAction("Create");
        }

        [HttpGet]
        [ImportModelStateFromTempData]
        public ActionResult Edit(int id)
        {
            var model = new CreateEditViewModel();
            var deliveryTime = deliveryTimeProvider.GetDeliveryTime(id);
            Mapper.DynamicMap(deliveryTime, model);
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
                    var deliveryTime = deliveryTimeProvider.GetDeliveryTime(model.Id);
                    Mapper.DynamicMap(model, deliveryTime);
                    deliveryTimeProvider.UpdateDeliveryTime(deliveryTime);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }

            return RedirectToAction("Edit");
        }
    }
}