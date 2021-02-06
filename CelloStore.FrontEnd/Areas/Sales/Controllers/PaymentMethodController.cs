using System;
using System.Web.Mvc;
using CelloStore.Data;
using CelloStore.FrontEnd.Attributes;
using CelloStore.FrontEnd.Base;
using CelloStore.Providers;
using CelloStore.ViewModels.Sales.PaymentMethod;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace CelloStore.FrontEnd.Areas.Sales.Controllers
{
    public class PaymentMethodController : AdminController
    {
        private readonly PaymentMethodProvider paymentMethodProvider;

        public PaymentMethodController(PaymentMethodProvider paymentMethodProvider)
        {
            this.paymentMethodProvider = paymentMethodProvider;
        }

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
                if (form["btnDelete"] != null)
                {
                    var paymentMethods = form.GetValues("chkDelete");
                    if (paymentMethods != null)
                    {
                        foreach (var paymentMethod in paymentMethods)
                        {
                            var paymentMethodId = Convert.ToInt32(paymentMethod);
                            paymentMethodProvider.DeletePaymentMethod(paymentMethodId);
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
            var query = paymentMethodProvider.List();
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
        [ValidateInput(false)]
        public ActionResult Create(FormCollection form, CreateEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var paymentMethod = new PaymentMethod();
                    paymentMethod.Name = model.Name;
                    paymentMethod.Notes = model.Notes;
                    paymentMethodProvider.AddOrUpdatePaymentMethod(paymentMethod);
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
            var paymentMethod = paymentMethodProvider.GetPaymentMethod(id);
            model.Id = paymentMethod.Id;
            model.Name = paymentMethod.Name;
            model.Notes = paymentMethod.Notes;
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
                    var paymentMethod = paymentMethodProvider.GetPaymentMethod(model.Id);
                    paymentMethod.Name = model.Name;
                    paymentMethod.Notes = model.Notes;
                    paymentMethodProvider.AddOrUpdatePaymentMethod(paymentMethod);
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