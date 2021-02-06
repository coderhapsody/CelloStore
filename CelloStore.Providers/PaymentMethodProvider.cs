using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using CelloStore.Data;
using CelloStore.Utilities.Extensions.Linq;
using CelloStore.ViewModels.Sales.PaymentMethod;

namespace CelloStore.Providers
{
    public class PaymentMethodProvider : BaseProvider
    {
        public PaymentMethodProvider(EntitiesModel context, IPrincipal principal) 
            : base(context, principal)
        {
        }

        public void AddOrUpdatePaymentMethod(PaymentMethod paymentMethod)
        {
            if (paymentMethod.Id == 0)
                context.Add(paymentMethod);

            SetAuditFields(paymentMethod);
            context.SaveChanges();
        }

        public IEnumerable<PaymentMethod> GetPaymentMethods()
        {
            return context.CreateDetachedCopy(context.PaymentMethods.ToList());
        }

        public IEnumerable<PaymentMethod> ListPaymentMethods(int pageIndex, int pageSize, string search, string sortExpression, out int rowCount)
        {
            var query = context.PaymentMethods.Where(cat => cat.Name.Contains(search));

            if (!string.IsNullOrEmpty(sortExpression))
                query = query.OrderBy(sortExpression);

            rowCount = query.Count();
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public IQueryable<ListPaymentMethodViewModel> List()
        {
            var query = from paymentMethod in context.PaymentMethods
                        select new ListPaymentMethodViewModel
                        {
                            Id = paymentMethod.Id,
                            Name = paymentMethod.Name
                        };
            return query;
        } 

        public PaymentMethod GetPaymentMethod(int id)
        {
            return context.PaymentMethods.SingleOrDefault(cat => cat.Id == id);
        }

        public void DeletePaymentMethod(int id)
        {
            var paymentMethod = GetPaymentMethod(id);
            if (paymentMethod != null)
            {
                context.Delete(paymentMethod);
                context.SaveChanges();
            }
        }

       
    }
}
