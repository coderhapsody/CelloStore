using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using CelloStore.Data;
using CelloStore.Utilities.Extensions.Linq;
using CelloStore.ViewModels.Administration.Role;
using CelloStore.ViewModels.Catalog.Category;

namespace CelloStore.Providers
{
    public class RoleProvider : BaseProvider
    {
        public RoleProvider(EntitiesModel context, IPrincipal principal) 
            : base(context, principal)
        {
        }

        public IEnumerable<RoleViewModel> GetRoles()
        {
            var query = from role in context.Roles
                        select new RoleViewModel
                        {
                            Id = role.Id,
                            Name = role.Name
                        };
            return query.ToList();
        }

        public void AddOrUpdateRole(Role role)
        {
            if (role.Id == 0)
                context.Add(role);
            SetAuditFields(role);
            context.SaveChanges();
        }

        public IQueryable<ListRoleViewModel> List()
        {
            var query = from role in context.Roles
                        select new ListRoleViewModel
                        {
                            Id = role.Id,
                            Name = role.Name,
                            IsSystemUsed = role.IsSystemRole
                        };
            return query;
        }

        public Role GetCategory(int id)
        {
            return context.Roles.SingleOrDefault(cat => cat.Id == id);
        }

        public void DeleteRole(int id)
        {
            var selectedRole = context.Roles.SingleOrDefault(role => role.Id == id);
            if (selectedRole != null)
            {
                context.Delete(selectedRole);
                context.SaveChanges();
            }
        }

        public Role GetRole(int id)
        {
            return context.Roles.SingleOrDefault(role => role.Id == id);
        }
    }
}
