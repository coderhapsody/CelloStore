using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using CelloStore.Data;
using CelloStore.Utilities.Extensions.Linq;
using CelloStore.ViewModels.Sales.AddressType;

namespace CelloStore.Providers
{
    public class AddressTypeProvider : BaseProvider
    {
        public AddressTypeProvider(EntitiesModel context, IPrincipal principal) : base(context, principal)
        {
        }

        public void AddOrUpdateAddressType(AddressType addressType)
        {
            if (addressType.Id == 0)
                context.Add(addressType);
            else
            {
                context.AttachCopy(addressType);
            }

            SetAuditFields(addressType);
            context.SaveChanges();
        }

        public IEnumerable<AddressType> GetAddressTypes(bool activeOnly = true)
        {
            var query = context.AddressTypes;
            if(activeOnly)
                query = query.Where(addressType => addressType.IsActive);

            return context.CreateDetachedCopy(query.ToList());
        }

        public IEnumerable<AddressType> ListAddressTypes(int pageIndex, int pageSize, string search, string sortExpression, out int rowCount)
        {
            var query = context.AddressTypes.Where(tag => tag.Name.Contains(search));

            if (!string.IsNullOrEmpty(sortExpression))
                query = query.OrderBy(sortExpression);

            rowCount = query.Count();
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public IQueryable<ListAddressTypeViewModel> List()
        {
            var query = from addrType in context.AddressTypes
                        select new ListAddressTypeViewModel
                        {
                            Id = addrType.Id,
                            Name = addrType.Name,
                            IsActive = addrType.IsActive ? "Yes" : "No"
                        };
            return query;
        }

        public AddressType GetAddressType(int id)
        {
            return context.CreateDetachedCopy(context.AddressTypes.SingleOrDefault(cat => cat.Id == id));
        }

        public void DeleteAddressType(int id)
        {
            var addressType = context.AddressTypes.Single(addrType => addrType.Id == id);
            if (addressType != null)
            {
                context.Delete(addressType);
                context.SaveChanges();
            }
        }
    }
}
