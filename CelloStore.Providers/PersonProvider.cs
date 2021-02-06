using System;
using System.Linq;
using System.Security.Principal;
using CelloStore.Data;
using CelloStore.ViewModels.Administration.Person;
using Telerik.OpenAccess;

namespace CelloStore.Providers
{
    public class PersonProvider : BaseProvider
    {
        public PersonProvider(EntitiesModel context, IPrincipal principal) : base(context, principal)
        {
        }

        public IQueryable<ListPersonViewModel> List()
        {
            var query = from person in context.People.Include(p => p.Role)
                        select new ListPersonViewModel
                        {
                            Id = person.Id,
                            Name = person.FirstName.Trim() + " " + person.LastName.Trim(),
                            Email = person.Email,
                            PhoneNumber = person.PhoneNumber,
                            IsActive = person.IsActive,
                            IsVerified = person.IsVerified,
                            LastLogOn = person.LastLogOn,
                            LastChangePassword = person.LastChangePassword,
                            RoleId =  person.RoleId,
                            RoleName = person.Role.Name
                        };
            return query;
        }

        public void DeletePerson(int[] personIds)
        {
            context.Delete(context.People.Where(person => personIds.Contains(person.Id)));
            context.SaveChanges();
        }

        public void DeletePerson(int personId)
        {
            var person = context.People.SingleOrDefault(p => p.Id == personId);
            if(person != null)
            {
                context.Delete(person);
                context.SaveChanges();
            }
        }

        public Person GetPerson(string userName)
        {
            return context.People.SingleOrDefault(person => person.Email == userName);
        }

        public Person GetPerson(int personId)
        {
            return context.People.SingleOrDefault(person => person.Id == personId);
        }

        public void UpdateAccountInfo(Person person)
        {
            SetAuditFields(person);
            context.SaveChanges();
        }

        public void RegisterAccount(Person person)
        {
            if(context.People.Any(p => p.Email == person.Email))
            {
                throw new CelloStoreException(string.Format("User Name with email {0} has been already registered.", person.Email));
            }

            person.RoleId = SecurityProvider.RoleCustomer;
            person.ChallangeCode = Guid.NewGuid().ToString().ToUpperInvariant();
            person.ChallangedWhen = DateTime.Now;
            SetAuditFields(person);
            context.Add(person);
            context.SaveChanges();
        }

        public void ResetPassword(int personId, string email)
        {
            
        }
    }
}
