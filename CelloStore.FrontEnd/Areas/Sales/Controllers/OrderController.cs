using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using CelloStore.Data;
using CelloStore.FrontEnd.Base;
using CelloStore.Providers;
using CelloStore.ViewModels.Common;
using CelloStore.ViewModels.MailTemplate;
using CelloStore.ViewModels.Sales.Order;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace CelloStore.FrontEnd.Areas.Sales.Controllers
{
    public class OrderController : AdminController
    {
        private readonly OrderProvider orderProvider;
        private readonly OrderStatusProvider orderStatusProvider;
        private readonly PersonProvider personProvider;
        public OrderController(OrderProvider orderProvider, OrderStatusProvider orderStatusProvider, PersonProvider personProvider)
        {
            this.orderProvider = orderProvider;
            this.orderStatusProvider = orderStatusProvider;
            this.personProvider = personProvider;
        }


        public ActionResult Index()
        {
            var model = new IndexViewModel();
            return View(model);
        }

        public ActionResult List([DataSourceRequest] DataSourceRequest request)
        {
            var queryOrder = orderProvider.ListOrders();
            return Json(queryOrder.ToDataSourceResult(request));
        }

        public ActionResult ListStatusHistory([DataSourceRequest] DataSourceRequest request, int orderId)
        {
            var queryStatusHistory = orderProvider.GetOrderStatusHistories(orderId);
            return Json(queryStatusHistory.ToDataSourceResult(request));
        }

        public ActionResult Edit(int id)
        {
            var model = new CreateEditViewModel();
            model.SelectedOrder = orderProvider.GetOrder(id);
            model.SelectedOrderDetails = orderProvider.GetOrderDetails(id);
            model.PaymentConfirmation = orderProvider.GetPaymentConfirmation(id);
            //model.StatusHistories = orderProvider.GetOrderStatusHistories(id, true);
            return View(model);
        }

        [ChildActionOnly]
        public string GetOrderStatusName(int orderStatusId)
        {
            var orderStatus = orderStatusProvider.GetOrderStatus(orderStatusId);
            if (orderStatus != null)
            {
                return orderStatus.Name;
            }
            return String.Empty;
        }

        [HttpGet]
        public ActionResult ChangeStatus(int orderId)
        {
            var model = new ChangeStatusViewModel();
            var order = orderProvider.GetOrder(orderId);
            model.CurrentStatusId = order.OrderStatusId;
            model.CurrentStatus = GetOrderStatusName(model.CurrentStatusId);

            return PartialView(model);
        }

        [HttpGet]
        public ActionResult GetOrderStatus(int orderStatusId)
        {
            var orderStatus = orderStatusProvider.GetOrderStatus(orderStatusId);
            return Json(orderStatus, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<ActionResult> ChangeStatus(FormCollection form, ChangeStatusViewModel model)
        {
            try
            {
                orderProvider.SetStatusOrder(model.OrderId, model.NewStatusId, model.Notes, model.SendMail);
                if (model.SendMail)
                {
                    var order = orderProvider.GetOrder(model.OrderId);
                    var orderStatus = orderStatusProvider.GetOrderStatus(model.NewStatusId);
                    var viewModel = orderProvider.GetOrderMail(order);

                    if (orderStatus != null)
                    {
                        var config = new TemplateServiceConfiguration();
                        using (var service = RazorEngineService.Create(config))
                        {
                            var body = service.RunCompile(
                                   orderStatus.EmailTemplate,
                                   "OrderMail",
                                   typeof(OrderMailViewModel),
                                   new { viewModel });
                            await orderProvider.NotifyOrderStatusUpdate(
                                ConfigurationInstance[ConfigurationKeys.SmtpServer],
                                Convert.ToInt32(ConfigurationInstance[ConfigurationKeys.SmtpPort]),
                                Convert.ToBoolean(ConfigurationInstance[ConfigurationKeys.SmtpSsl]),
                                !Convert.ToBoolean(ConfigurationInstance[ConfigurationKeys.SmtpAuthentication]),
                                ConfigurationInstance[ConfigurationKeys.SmtpUserName],
                                ConfigurationInstance[ConfigurationKeys.SmtpPassword],
                                ConfigurationInstance[ConfigurationKeys.SystemSenderName],
                                ConfigurationInstance[ConfigurationKeys.SystemSenderAddress],
                                viewModel.CustomerMail,
                                body);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 406;
                Response.Write(ex.Message);
                return null;
            }

            return Json(model);
        }

        [HttpGet]
        public ActionResult GetCurrentOrderStatus(int orderId)
        {
            var order = orderProvider.GetOrder(orderId);
            var orderStatus = orderStatusProvider.GetOrderStatus(order.OrderStatusId);
            return Json(new
            {
                OrderStatusId = orderStatus.Id,
                OrderStatusName = orderStatus.Name
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOrderStatuses()
        {
            var list = orderStatusProvider.List();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult VerifyPayment(int paymentConfirmationId)
        {
            try
            {
                orderProvider.VerifyPaymentConfirmation(paymentConfirmationId);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
            return Json(true);
        }
    }
}