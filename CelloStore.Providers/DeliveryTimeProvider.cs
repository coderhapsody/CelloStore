using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using CelloStore.Data;

namespace CelloStore.Providers
{
    public class DeliveryTimeProvider : BaseProvider
    {
        public DeliveryTimeProvider(EntitiesModel context, IPrincipal principal) 
            : base(context, principal)
        {
        }

        public void UpdateDeliveryTime(DeliveryTime deliveryTime)
        {
            if (deliveryTime.Id == 0)
                context.Add(deliveryTime);
            SetAuditFields(deliveryTime);
            context.SaveChanges();
        }

        public IEnumerable<DeliveryTime> GetDeliveryTimes()
        {
            return context.DeliveryTimes.Where(delivery => delivery.IsActive).ToList();
        }

        public IQueryable<DeliveryTime> List()
        {
            var query = context.DeliveryTimes;
            return query;
        }

        public DeliveryTime GetDeliveryTime(int id)
        {
            return context.DeliveryTimes.SingleOrDefault(cat => cat.Id == id);
        }
    }
}
