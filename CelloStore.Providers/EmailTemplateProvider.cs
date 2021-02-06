using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using CelloStore.Data;

namespace CelloStore.Providers
{
    public enum EmailTemplates
    {
        Registration = 1,
        ForgetPassword = 2
    }

    public class EmailTemplateProvider : BaseProvider
    {
        public EmailTemplateProvider(EntitiesModel context, IPrincipal principal) : base(context, principal)
        {
        }


        public IQueryable<EmailTemplate> List()
        {
            return context.EmailTemplates;
        }


        public EmailTemplate GetEmailTemplate(EmailTemplates emailTemplate)
        {
            return context.EmailTemplates.SingleOrDefault(template => template.Id == (int)emailTemplate);
        }

        public EmailTemplate GetEmailTemplate(int id)
        {
            return context.EmailTemplates.SingleOrDefault(template => template.Id == id);
        }

        public void UpdateEmailTemplate(EmailTemplate emailTemplate)
        {
            SetAuditFields(emailTemplate);
            context.SaveChanges();
        }
    }
}
