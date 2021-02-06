using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CelloStore.FrontEnd.Base;
using CelloStore.Providers;
using CelloStore.ViewModels.Sales.OrderStatus;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace CelloStore.FrontEnd.Areas.Sales.Controllers
{
    [Authorize]
    public class OrderStatusController : AdminController
    {
        private readonly OrderStatusProvider orderStatusProvider;

        public OrderStatusController(OrderStatusProvider orderStatusProvider)
        {
            this.orderStatusProvider = orderStatusProvider;
        }

        public ActionResult Index()
        {
            var model = new IndexViewModel();
            return View(model);
        }

        public ActionResult List([DataSourceRequest] DataSourceRequest request)
        {
            var query = orderStatusProvider.List();
            return Json(query.ToDataSourceResult(request));
        }

        public ActionResult Edit(int id)
        {
            var model = new EditViewModel();
            var orderStatus = orderStatusProvider.GetOrderStatus(id);
            Mapper.DynamicMap(orderStatus, model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection form, EditViewModel model)
        {
            var orderStatus = orderStatusProvider.GetOrderStatus(model.Id);
            Mapper.DynamicMap(model, orderStatus);
            orderStatusProvider.UpdateOrderStatus(orderStatus);

            return RedirectToAction("Index");
        }
    }
}