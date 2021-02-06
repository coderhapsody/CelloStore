using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Linq;
using CelloStore.Data;
using CelloStore.Utilities.Extensions.Linq;

namespace CelloStore.Providers
{
    public class SecurityProvider : PersonProvider
    {
        public static readonly int RoleAdministrator = 1;
        public static readonly int RoleCustomer = 2;

        public SecurityProvider(EntitiesModel context, IPrincipal principal)
            : base(context, principal)
        {
        }

        #region Roles
        public void AddOrUpdateRole(Role role)
        {
            if (role.Id == 0)
                context.Add(role);

            SetAuditFields(role);
            context.SaveChanges();
        }

        public IEnumerable<Role> GetRoles()
        {
            return context.Roles.ToList();
        }

        public IEnumerable<Role> ListRoles(int pageIndex, int pageSize, string search, string sortExpression, out int rowCount)
        {
            var query = context.Roles.Where(cat => cat.Name.Contains(search));

            if (!string.IsNullOrEmpty(sortExpression))
                query = query.OrderBy(sortExpression);

            rowCount = query.Count();
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public Role GetRole(int id)
        {
            return context.Roles.SingleOrDefault(cat => cat.Id == id);
        }

        public void DeleteRole(int id)
        {
            var role = GetRole(id);
            if (role != null)
            {
                context.Delete(role);
                context.SaveChanges();
            }
        }
        #endregion

        #region User

        public bool IsAdministrator(string email)
        {
            var person = GetPerson(email);
            return person != null && person.RoleId == RoleAdministrator;
        }

        public bool ValidateUser(string email, string password)
        {
            var user = context.People.SingleOrDefault(p => p.Email == email && p.Password == password);
            return user != null;
        }

        public void ChangePassword(string email, string newPassword)
        {
            var user = context.People.SingleOrDefault(p => p.Email == email);
            if (user != null)
            {
                user.Password = newPassword;
                user.LastChangePassword = DateTime.Now;
                SetAuditFields(user);
                context.SaveChanges();
            }
        }
        #endregion

        private string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

        public string ResetPassword(Person person, string email)
        {
            string newPassword = GenerateRandomPassword();
            person.Password = newPassword;
            context.SaveChanges();

            return newPassword;
        }

        public void LogInUser(string userName)
        {
            var user = context.People.SingleOrDefault(p => p.Email == userName);
            if (user != null)
            {
                user.LastLogOn = DateTime.Now;                                
                context.SaveChanges();
            }
        }

        public Person FindUserByEmailAddress(string email)
        {
            return context.People.FirstOrDefault(p => p.Email == email && p.IsActive);

        }

        public void ActivateAccount(string email, string activationCode)
        {
            var user = context.People.SingleOrDefault(p => p.Email == email);
            if(user != null)
            {
                user.ActivatedWhen = DateTime.Now;
                context.SaveChanges();
            }
        }

        public bool ValidateRegistration(string email, string activationCode)
        {
            var user = context.People.SingleOrDefault(p => p.Email == email);
            if(user != null)
            {
                return user.ChallangeCode.ToUpperInvariant() == activationCode.Trim().ToUpperInvariant();
            }
            return false;
        }

        public bool IsAccountActivated(string userName)
        {
            var user = context.People.SingleOrDefault(p => p.Email == userName);
            if(user != null)
            {
                return user.ActivatedWhen.HasValue;
            }
            return false;
        }
    }
}
