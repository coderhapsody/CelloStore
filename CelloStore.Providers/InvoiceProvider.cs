using CelloStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CelloStore.Providers
{
    public enum InvoiceStatus 
    {
        NotActive = 0,
        Active = 1
    }

    public class InvoiceProvider : BaseProvider
    {
        private OrderProvider orderProvider;

        public InvoiceProvider(EntitiesModel context, IPrincipal principal, OrderProvider orderProvider)
            : base(context, principal)
        {
            this.orderProvider = orderProvider;
        }

        public IEnumerable<string> CreateInvoiceFromOrder(int orderId)
        {
            var order = orderProvider.GetOrder(orderId);
            if (order == null)
                throw new CelloStoreException("Order is not found.");

            if (order.OrderStatusId == (int)OrderStatuses.Cancelled)
                throw new CelloStoreException(String.Format("Order {0} has been cancelled.", order.OrderNo));

            foreach (OrderDetail orderDetail in order.OrderDetails)
            {
                string invoiceNo = CreateInvoiceFromOrderDetail(orderDetail.Id);
                yield return invoiceNo;
            }
        }

        public string CreateInvoiceFromOrderDetail(int orderDetailId)
        {
            Invoice invoice = context.Invoices.SingleOrDefault(inv => inv.OrderDetailId == orderDetailId);
            if (invoice != null)
            {
                return invoice.InvoiceNo;
            }
            return CreateInvoice(orderDetailId);
        }

        private string CreateInvoice(int orderDetailId)
        {
            OrderDetail orderDetail = context.OrderDetails.SingleOrDefault(od => od.Id == orderDetailId);
            
            Invoice invoice = new Invoice();
            invoice.InvoiceNo = GenerateInvoiceNumber();
            invoice.OrderDetailId = orderDetailId;
            invoice.OrderId = orderDetail.OrderId;
            invoice.Date = DateTime.Today;
            invoice.Status = (int)InvoiceStatus.Active;
            invoice.Amount = orderDetail.Qty * orderDetail.UnitPrice - orderDetail.DiscValue;
            invoice.CreatedBy = principal.Identity.Name;
            invoice.CreatedWhen = DateTime.Now;
            invoice.ChangedBy = principal.Identity.Name;
            invoice.ChangedWhen = invoice.CreatedWhen;
            invoice.DeliveryNo = "";
            context.Add(invoice);
            context.SaveChanges();
            return invoice.InvoiceNo;
        }

        private string GenerateInvoiceNumber()
        {
            string year = DateTime.Today.ToString("yyyy");
            string month = DateTime.Today.ToString("MM");
            string yearMonth = String.Format("{0}{1}", year, month);
            AutoNumber autoNumber = context.AutoNumbers.FirstOrDefault(an => an.FormCode == "INV" && an.Year == Convert.ToInt32(yearMonth));
            if (autoNumber == null)
            {
                autoNumber = new AutoNumber();
                autoNumber.FormCode = "INV";
                autoNumber.Year = Convert.ToInt32(yearMonth);
                autoNumber.LastNumber = 0;
                context.Add(autoNumber);
            }
            autoNumber.LastNumber++;
            string invoiceNo = String.Format("INV/{0}{1}/{2}", year, month, autoNumber.LastNumber.ToString("0000"));
            return invoiceNo;
        }

        public void SetInvoiceStatus(int invoiceId, InvoiceStatus invoiceStatus)
        {
            Invoice invoice = context.Invoices.SingleOrDefault(inv => inv.Id == invoiceId);
            if (invoice != null)
            {
                invoice.Status = (int)invoiceStatus;
                invoice.LastChangedStatusBy = principal.Identity.Name;
                invoice.LastChangedStatusWhen = DateTime.Now;
                context.SaveChanges();
            }
        }     
    }
}
