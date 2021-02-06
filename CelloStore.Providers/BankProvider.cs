using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using CelloStore.Data;

namespace CelloStore.Providers
{
    public class BankProvider : BaseProvider
    {
        public BankProvider(EntitiesModel context, IPrincipal principal) 
            : base(context, principal)
        {
        }

        public IEnumerable<Bank> GetBanks()
        {
            return context.CreateDetachedCopy(context.Banks.Where(bank => bank.IsActive).ToList());
        }
    }
}
