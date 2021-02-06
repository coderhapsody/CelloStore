using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using CelloStore.Data;

namespace CelloStore.Providers
{
    public class OrderStatusProvider : BaseProvider
    {
        public OrderStatusProvider(EntitiesModel context, IPrincipal principal) : base(context, principal)
        {
        }

        public IQueryable<OrderStatus> List()
        {
            return context.OrderStatus;
        } 

        public OrderStatus GetOrderStatus(int orderStatusId)
        {
            return context.OrderStatus.SingleOrDefault(orderStatus => orderStatus.Id == orderStatusId);
        }

        public void UpdateOrderStatus(OrderStatus orderStatus)
        {
            orderStatus.ChangedBy = principal.Identity.Name;
            orderStatus.ChangedWhen = DateTime.Now;
            
            context.SaveChanges();
        }

        public string GetEmailTemplate(int orderStatusId)
        {
            var selectedOrderStatus = context.OrderStatus.SingleOrDefault(orderStatus => orderStatus.Id == orderStatusId);
            return selectedOrderStatus != null ? selectedOrderStatus.EmailTemplate : String.Empty;
        }
    }
}
