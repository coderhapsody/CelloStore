using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using CelloStore.Data;
using CelloStore.ViewModels.Reports;
using Telerik.OpenAccess;

namespace CelloStore.Providers
{
    public class ReportProvider 
    {
        public ReportProvider()
        {            
        }

        public string ConnectionString { get; set; }

        public IEnumerable<DeliveryOrderViewModel> ReportDeliveryOrder(int orderId)
        {
            var model = new DeliveryOrderViewModel();
            using (var context = new EntitiesModel(ConnectionString))
            {
                var order = context.Orders.SingleOrDefault(o => o.Id == orderId);
                if(order != null)
                {
                    model.OrderNo = order.OrderNo;
                    model.RecipientName = order.RecipientName;
                    model.DeliveryAddress = order.RecipientAddress;
                    model.DeliveryDateTime = order.DeliveryDate.GetValueOrDefault();
                    model.Notes = order.Notes;                    
                }
            }
            yield return model;
        }

        public IEnumerable<DeliveryOrderDetailViewModel> ReportDeliveryOrderDetail(int orderId)
        {
            using (var context = new EntitiesModel(ConnectionString))
            {
                var queryDetail = from detail in context.OrderDetails.Include(o => o.Product)
                                  where detail.OrderId == orderId
                                  select new DeliveryOrderDetailViewModel
                                  {
                                      ProductNo = detail.Product.Code,
                                      Description = detail.Product.Description,
                                      Qty = detail.Qty
                                  };

                var list = queryDetail.ToList();
                foreach(var deliveryOrderDetailViewModel in list)
                {
                    yield return deliveryOrderDetailViewModel;
                }

            }            
        }
    }
}
