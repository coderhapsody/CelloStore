using System.Diagnostics;
using System.Security.Principal;
using CelloStore.Data;

namespace CelloStore.Providers
{
    public abstract class BaseProvider
    {
        protected EntitiesModel context;
        protected IPrincipal principal;
            
        protected BaseProvider(EntitiesModel context, IPrincipal principal)
        {
            this.context = context;
            this.principal = principal;
        }

        [DebuggerStepThrough]
        protected void SetAuditFields(dynamic entity)
        {
            if (entity.Id == 0)
                EntityHelper.SetAuditFieldsForInsert(entity, principal.Identity.Name);
            else
                EntityHelper.SetAuditFieldsForUpdate(entity, principal.Identity.Name);
        }
    }
}
