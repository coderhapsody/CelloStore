using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using CelloStore.Providers;
using CelloStore.ViewModels;

namespace CelloStore.FrontEnd.Helpers
{
    public class CartManager
    {
        private readonly ProductProvider productProvider;
        private readonly IPrincipal principal;
        private readonly AreaProvider areaProvider;
        private readonly DeliveryTimeProvider deliveryTimeProvider;

        public CartManager(
            ProductProvider productProvider,
            AreaProvider areaProvider,
            DeliveryTimeProvider deliveryTimeProvider,
            IPrincipal principal)
        {
            this.productProvider = productProvider;
            this.principal = principal;
            this.areaProvider = areaProvider;
            this.deliveryTimeProvider = deliveryTimeProvider;
        }

        public void DoCheckOut(DateTime deliveryDate, byte deliveryTime, string voucherCode)
        {
            var delivTime = deliveryTimeProvider.GetDeliveryTime(Convert.ToInt32(deliveryTime));
            MyCart.DeliveryDate = deliveryDate;
            MyCart.DeliveryTime = deliveryTime;
            MyCart.DeliveryTimeName = delivTime.Name;
            MyCart.VoucherCode = voucherCode;
        }

        public CartViewModel MyCart
        {
            get
            {
                var myCart = HttpContext.Current.Session["MyCart"] as CartViewModel;
                if (myCart == null)
                {
                    myCart = new CartViewModel();
                    myCart.OrderAreaId = areaProvider.DefaultArea.Id;
                    myCart.UserName = principal != null ? principal.Identity.Name : String.Empty;
                    myCart.OrderDetails = new List<OrderDetailViewModel>();
                    HttpContext.Current.Session["MyCart"] = myCart;
                }
                return myCart;
            }
        }

        public void SetOrderArea(int areaId)
        {
            MyCart.OrderAreaId = areaId;
        }

        public int CountItemsInCart()
        {
            return MyCart.OrderDetails.Count;
        }

        public CartViewModel GetCart()
        {
            return MyCart;
        }

        public void SetCartUserName()
        {
            MyCart.UserName = principal.Identity.Name;
        }

        public void AddProductToCart(string productCode, int qty)
        {
            var product = productProvider.GetProduct(productCode);
            if (product != null)
            {
                var orderInCart = MyCart.OrderDetails.FirstOrDefault(cart => cart.ProductCode == productCode);
                if(orderInCart != null)
                {
                    orderInCart.Qty += qty;
                }
                else
                {
                    var orderModel = new OrderDetailViewModel
                    {
                        ProductCode = product.Code,
                        ProductId = product.Id,
                        ProductName = product.Name,
                        UnitPrice = product.UnitPrice,
                        Qty = qty,
                        LineNo = MyCart.OrderDetails.Count + 1
                    };

                    MyCart.OrderDetails.Add(orderModel);
                }
            }
        }

        public void RemoveProductFromCart(int lineNo)
        {
            var productInCart = MyCart.OrderDetails.SingleOrDefault(o => o.LineNo == lineNo);
            MyCart.OrderDetails.Remove(productInCart);
        }

        public void UpdateOrderLineQty(int lineNo, int qty)
        {
            var productInCart = MyCart.OrderDetails.SingleOrDefault(o => o.LineNo == lineNo);
            if (productInCart != null)
            {
                productInCart.Qty = qty;
            }
        }

        public void EmptyCart()
        {
            MyCart.OrderDetails.Clear();
        }

        public OrderDetailViewModel GetOrderLine(int lineNo)
        {
            return MyCart.OrderDetails.SingleOrDefault(o => o.LineNo == lineNo);
        }

        public void CheckOutOrder()
        {
            // create order based on current cart
        }
    }
}