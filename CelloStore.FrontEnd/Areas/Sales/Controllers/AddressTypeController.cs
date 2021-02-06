using System;
using System.Web.Mvc;
using CelloStore.Data;
using CelloStore.FrontEnd.Attributes;
using CelloStore.FrontEnd.Base;
using CelloStore.Providers;
using CelloStore.ViewModels.Sales.AddressType;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace CelloStore.FrontEnd.Areas.Sales.Controllers
{
    public class AddressTypeController : AdminController
    {
        private readonly AddressTypeProvider addressTypeProvider;

        public AddressTypeController(AddressTypeProvider addressTypeProvider)
        {
            this.addressTypeProvider = addressTypeProvider;
        }

        public ActionResult Index(int? page, int? pageSize, string search, string sort)
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
                if (form["btnDelete"] != null)
                {
                    var addressTypes = form.GetValues("chkDelete");
                    if (addressTypes != null)
                    {
                        foreach (var addressType in addressTypes)
                        {
                            var productId = Convert.ToInt32(addressType);
                            addressTypeProvider.DeleteAddressType(productId);
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
            var query = addressTypeProvider.List();
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
                    var addressType = new AddressType();
                    addressType.Name = model.Name;
                    addressTypeProvider.AddOrUpdateAddressType(addressType);
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
            var addressType = addressTypeProvider.GetAddressType(id);
            model.Id = addressType.Id;
            model.Name = addressType.Name;
            model.IsActive = addressType.IsActive;
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
                    var addressType = addressTypeProvider.GetAddressType(model.Id);
                    addressType.Name = model.Name;
                    addressType.IsActive = model.IsActive;
                    addressTypeProvider.AddOrUpdateAddressType(addressType);
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