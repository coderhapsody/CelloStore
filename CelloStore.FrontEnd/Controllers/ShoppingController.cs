using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Mvc;
using CelloStore.Data;
using CelloStore.FrontEnd.Attributes;
using CelloStore.FrontEnd.Base;
using CelloStore.FrontEnd.Helpers;
using CelloStore.Providers;
using CelloStore.Utilities.Helpers;
using CelloStore.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using ResultOrDefault;

namespace CelloStore.FrontEnd.Controllers
{
    public class ShoppingController : BaseController
    {
        private readonly CategoryProvider categoryProvider;
        private readonly ProductProvider productProvider;
        private readonly TagProvider tagProvider;
        private readonly AreaProvider areaProvider;
        private readonly CartManager cartManager;
        private readonly PaymentMethodProvider paymentMethodProvider;
        private readonly AddressTypeProvider addressTypeProvider;
        private readonly OrderProvider orderProvider;
        private readonly PersonProvider personProvider;
        private readonly DeliveryTimeProvider deliveryTimeProvider;
        private readonly BankProvider bankProvider;
        public ShoppingController(
            CategoryProvider categoryProvider,
            ProductProvider productProvider,
            TagProvider tagProvider,
            AreaProvider areaProvider,
            PaymentMethodProvider paymentMethodProvider,
            AddressTypeProvider addressTypeProvider,
            OrderProvider orderProvider,
            PersonProvider personProvider,
            CartManager cartManager,
            BankProvider bankProvider,
            DeliveryTimeProvider deliveryTimeProvider)
        {
            this.categoryProvider = categoryProvider;
            this.productProvider = productProvider;
            this.tagProvider = tagProvider;            
            this.areaProvider = areaProvider;
            this.cartManager = cartManager;
            this.addressTypeProvider = addressTypeProvider;
            this.paymentMethodProvider = paymentMethodProvider;
            this.personProvider = personProvider;
            this.orderProvider = orderProvider;
            this.deliveryTimeProvider = deliveryTimeProvider;
            this.bankProvider = bankProvider;
        }

        [AllowAnonymous]
        public ActionResult ViewCart()
        {
            var deliveryDate = DateTime.Today.AddDays(1);
            var deliveryTime = 1;

            if (deliveryDate.Subtract(DateTime.Today).Days == 1 && DateTime.Now.Hour > 12 && deliveryTime == 1)
            {
                deliveryTime = 2;
            }

            ViewBag.DeliveryDate = deliveryDate;
            ViewBag.DeliveryTime = deliveryTime;

            CartViewModel model = cartManager.MyCart;
            return View(model);        
        }


        [HttpPost]
        [Authorize]
        public ActionResult ViewCart(FormCollection form)
        {
            var deliveryDate = Convert.ToDateTime(form["deliveryDate"], new CultureInfo("id-ID"));
            var deliveryTime = Convert.ToByte(form["deliveryTime"]);
            var voucherCode = form["voucherCode"];
            cartManager.DoCheckOut(deliveryDate, deliveryTime, voucherCode);

            return RedirectToAction("CheckOut");
        }

        
        public ActionResult OrderHistory()
        {            
            var model = new OrderHistoryViewModel();
            var person = personProvider.GetPerson(CurrentUserName);
            model.PersonId = person.Id;
            return View(model);
        }

        [HttpPost]
        public ActionResult ListOrderHistory([DataSourceRequest] DataSourceRequest request, int personId)
        {
            var orderHistoryList = orderProvider.ListPersonOrders(personId);
            return Json(orderHistoryList.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult ListStatusHistory([DataSourceRequest] DataSourceRequest request, int orderId)
        {
            var queryStatusHistory = orderProvider.GetOrderStatusHistories(orderId);
            return Json(queryStatusHistory.ToDataSourceResult(request));
        }

        [HttpGet]
        public ActionResult ListDeliveryTimes()
        {
            var deliveryTimes = deliveryTimeProvider.GetDeliveryTimes();
            return Json(deliveryTimes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrderHistoryDetail(int orderId)
        {
            var viewModel = new OrderHistoryDetailViewModel();
            var order = orderProvider.GetOrder(orderId);
            viewModel.SelectedOrder = order;
            viewModel.SelectedOrderDetails = orderProvider.GetOrderDetails(orderId);
            viewModel.DeliveryTimeName = deliveryTimeProvider.GetDeliveryTime(order.DeliveryTime).Name;
            //viewModel.StatusHistories = orderProvider.GetOrderStatusHistories(orderId);
            viewModel.PaymentConfirmation = orderProvider.GetPaymentConfirmation(orderId);
            if(!orderProvider.ValidateOrderOwner(orderId, CurrentUserName))
            {
                return RedirectToAction("OrderHistory");
            }
            
            return PartialView(viewModel);
        }

        public ActionResult ProductDetail(string areaName, int id)
        {
            var product = productProvider.GetProductDetail(areaName, id);
            TempData["areaName"] = areaName;
            return View(product);
        }

        [HttpPost]
        public ActionResult ProductDetail(FormCollection form)
        {
            int qty = Convert.ToInt32(form["qty"]);
            string productCode = form["Code"];
            cartManager.AddProductToCart(productCode, qty);

            return RedirectToAction("ProductDetail");
        }

        public ActionResult Catalog(string areaName, string categoryName)
        {
            if(String.IsNullOrEmpty(areaName))
                areaName = areaProvider.DefaultArea.Name;

            if (!areaProvider.IsActive(areaName))
                return RedirectToAction("Catalog", new { areaName = areaProvider.DefaultArea.Name, categoryName } );

            TempData["areaName"] = areaName;
            TempData["categoryName"] = categoryName;

            return View();
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public ActionResult Category(string areaName)
        {
            var categories = new List<CategoryViewModel>();
            var rootCategories = categoryProvider.GetCategories(null);
            foreach (var rootCategory in rootCategories)
            {
                var model = new CategoryViewModel
                {
                    RootCategory = rootCategory,
                    ChildCategories = categoryProvider.GetCategories(rootCategory.Id)
                };
                categories.Add(model);
            }
            TempData["areaName"] = areaName;
            return PartialView("_Category", categories);
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public ActionResult Products(string areaName, string categoryName)
        {
            var pageIndex = Request["page"] ?? "1";
            var model = new ProductViewModel();
            int rowCount;
            
            var pageSize = Convert.ToInt32(ConfigurationInstance[ConfigurationKeys.PageSize]);            
            model.CurrentCategory = categoryProvider.GetCategoryByName(categoryName, true);
            //model.CurrentTag = null;
            model.Products = productProvider.GetPagedProducts(
               areaName, categoryName, Convert.ToInt32(pageIndex), pageSize, out rowCount).ToList();
            model.RowCount = rowCount;
            model.PageSize = pageSize;            
            return PartialView("_Products", model);
        }

        [HttpPost]
        public ActionResult AddItemToCart(string productCode, int qty, string areaName)
        {
            var area = areaProvider.GetAreaByName(areaName);
            if (area == null)
                return RedirectToAction("InvalidCheckOutProcess");

            cartManager.SetOrderArea(area.Id);
            cartManager.AddProductToCart(productCode, qty);
            return Json(cartManager.CountItemsInCart(), JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        public ActionResult CountCartItems()
        {
            return Json(cartManager.CountItemsInCart(), JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]        
        public ActionResult Area(string areaName)
        {
            var areas = areaProvider.GetAreas(true);
            ViewBag.areaName = areaName;
            return PartialView("_Area", areas);
        }

        [Authorize]
        [HttpGet]
        public ActionResult CheckOut()
        {
            var viewModel = new CheckOutViewModel();
            if(cartManager.CountItemsInCart() == 0)
            {
                return RedirectToAction("ViewCart");
            }
            viewModel.AddressTypes = addressTypeProvider.GetAddressTypes();
            viewModel.PaymentMethods = paymentMethodProvider.GetPaymentMethods();
            viewModel.Cart = cartManager.MyCart;
            var deliveryArea = areaProvider.GetArea(cartManager.MyCart.OrderAreaId);            
            viewModel.DeliveryArea = deliveryArea.Name;
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CheckOut(CheckOutViewModel model)
        {
            if(ModelState.IsValid)
            {
                var order = ProcessCheckOut(model, cartManager.MyCart);
                var orderNo = orderProvider.CreateOrderFromCart(order);
                var encryptedOrderNo = RijndaelHelper.Encrypt(orderNo,
                    ConfigurationInstance[ConfigurationKeys.CryptographyKey]);
                string returnUrl = String.Format("{0}?o={1}", Url.Action("CheckOutSuccess"), encryptedOrderNo);                

                return Json(new
                {
                    Error = false,
                    OrderNo = orderNo,
                    ReturnUrl = returnUrl
                });
            }

            var allErrors = ModelState.Values.SelectMany(v => v.Errors).Select(o => o.ErrorMessage);
            return Json(new 
            {
                Error = true,
                Errors = allErrors
            });
        }

        [NonAction]
        private Order ProcessCheckOut(CheckOutViewModel model, CartViewModel myCart)
        {
            var person = personProvider.GetPerson(CurrentUserName);
            var order = new Order();            
            AutoMapper.Mapper.DynamicMap(model, order);
            order.OrderDate = DateTime.Today;
            order.OrderStatusId = (int) OrderStatuses.Unpaid;
            order.PersonId = person.Id;
            order.UniquePayment = 2;
            order.DeliveryDate = myCart.DeliveryDate;
            order.DeliveryTime = myCart.DeliveryTime;
            order.OrderAreaId = myCart.OrderAreaId;
            foreach(var itemInCart in myCart.OrderDetails)
            {
                var orderDetail = new OrderDetail();
                orderDetail.ProductId = itemInCart.ProductId;
                orderDetail.Qty = itemInCart.Qty;
                orderDetail.UnitPrice = itemInCart.UnitPrice;
                orderDetail.DiscValue = itemInCart.DiscValue;
                orderDetail.Order = order;

                order.OrderDetails.Add(orderDetail);                
            }

            

            return order;
        }


        [HttpPost]
        public ActionResult Area(FormCollection form)
        {
            cartManager.EmptyCart();
            string areaName = form["areaName"];
            var area = areaProvider.GetAreaByName(areaName);
            if(area == null)
                return RedirectToAction("InvalidCheckOutProcess");
            cartManager.SetOrderArea(area.Id);
            return RedirectToAction("Catalog", new { areaName });
        }

        [HttpPost]
        public ActionResult EmptyCart()
        {
            cartManager.EmptyCart();
            return Json(cartManager.CountItemsInCart(), JsonRequestBehavior.DenyGet);
        }
        
        public ActionResult RemoveFromCart(int lineNo)
        {
            cartManager.RemoveProductFromCart(lineNo);
            return RedirectToAction("ViewCart");
        }

        public ActionResult CheckOutSuccess(string o)
        {
            cartManager.EmptyCart();

            var model = new CheckOutSuccessViewModel();

            try
            {
                string orderNo = RijndaelHelper.Decrypt(o, ConfigurationInstance[ConfigurationKeys.CryptographyKey]);
                var order = orderProvider.GetOrder(orderNo);
                model.SubmittedOrder = order;
            }
            catch (CryptographicException ex)
            {
                Logger.FatalException(ex.Message, ex);
                return RedirectToAction("InvalidCheckOutProcess");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult VerifyDeliveryDate(DateTime date)
        {
            return Json(new
            {
                IsValid = date > DateTime.Today,
                ValidDate = DateTime.Today.AddDays(1)
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InvalidCheckOutProcess()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetPaymentMethodNotes(int paymentMethodId)
        {
            var paymentMethod = paymentMethodProvider.GetPaymentMethod(paymentMethodId);
            string paymentNotes = paymentMethod != null ? paymentMethod.Notes : String.Empty;
            paymentNotes = paymentNotes ?? String.Empty;                
            return Json(paymentNotes, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public string GetOrderStatusName(int orderStatusId)
        {
            return ((OrderStatuses)Enum.Parse(typeof(OrderStatuses), Convert.ToString(orderStatusId))).ToString();
        }

        [HttpGet]
        public ActionResult VerifyDeliveryTime(DateTime date, int time)
        {
            if(date.Subtract(DateTime.Today).Days == 1 && DateTime.Now.Hour > 12 && time == 1)
            {
                return Json(new {IsValid = false, ValidTime = 2}, JsonRequestBehavior.AllowGet);
            }

            return Json(new { IsValid = true, ValidTime = time }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        [ImportModelStateFromTempData]
        public ActionResult PaymentConfirmation()
        {
            var model = new PaymentConfirmationViewModel();
            model.PaymentDate = DateTime.Today;
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetBanks()
        {
            var banks =
                bankProvider.GetBanks()
                    .Select(bank => new {bank.Id, Name = bank.Name + " " + bank.AccountNumber});
            return Json(banks, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        [ExportModelStateToTempData]
        public ActionResult PaymentConfirmation(FormCollection form, PaymentConfirmationViewModel model)
        {
            if(ModelState.IsValid)
            {
                bool paymentConfirmationIsValid = orderProvider.ValidateOrderPayment(
                    model.OrderNo,
                    model.PayAmount);
                if(paymentConfirmationIsValid)
                {
                    if(!orderProvider.IsOrderPaymentConfirmed(model.OrderNo))
                    {
                        orderProvider.CreatePaymentConfirmation(model.OrderNo, model.BankId, 2, model.PaymentDate,
                            model.SenderName, model.PayAmount, null);
                        string encryptedOrderNo = RijndaelHelper.Encrypt(model.OrderNo, CryptographyKey);
                        return RedirectToAction("PaymentConfirmationSuccess", new {token = encryptedOrderNo});
                    }
                    ModelState.AddModelError(String.Empty, 
                        String.Format("Payment confirmation for order {0} has been created already.", model.OrderNo));
                    return RedirectToAction("PaymentConfirmation");
                }
                ModelState.AddModelError(String.Empty, String.Format("Invalid Payment for Order {0}", model.OrderNo));
                return RedirectToAction("PaymentConfirmation");
            }

            return View(model);
        }


        public ActionResult PaymentConfirmationSuccess(string token)
        {
            try
            {
                string decryptedOrderNo = RijndaelHelper.Decrypt(token, CryptographyKey);
                ViewBag.OrderNo = decryptedOrderNo;
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return RedirectToAction("PaymentConfirmation");
            }

            return View();
        }
    }
}