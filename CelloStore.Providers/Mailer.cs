using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using CelloStore.Utilities.Extensions.String;

namespace CelloStore.Providers
{
    public class Mailer
    {
        private readonly ConfigurationProvider configurationProvider;

        private readonly SmtpClient smtpClient;
        private readonly MailAddress senderInfo;

        public Mailer(ConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;

            var smtpServer = configurationProvider[ConfigurationKeys.SmtpServer];
            var smtpPort = Convert.ToInt32(configurationProvider[ConfigurationKeys.SmtpPort]);
            var useSsl = Convert.ToBoolean(configurationProvider[ConfigurationKeys.SmtpSsl]);
            var defaultCredential = !Convert.ToBoolean(configurationProvider[ConfigurationKeys.SmtpAuthentication]);
            var userName = configurationProvider[ConfigurationKeys.SmtpUserName];
            var password = configurationProvider[ConfigurationKeys.SmtpPassword];
            var senderName = configurationProvider[ConfigurationKeys.SystemSenderName];
            var senderAddress = configurationProvider[ConfigurationKeys.SystemSenderAddress];
            senderInfo = new MailAddress(senderAddress, senderName);
            smtpClient = new SmtpClient(smtpServer, smtpPort)
            {
                EnableSsl = useSsl,
                UseDefaultCredentials = defaultCredential
            };

            if (!defaultCredential)
            {
                smtpClient.Credentials = new NetworkCredential(userName, password);
            }
        }

        public async Task SendMail(string personName, string destinationMailAddress, string bcc, string subject, string body)
        {
            var mailMessage = new MailMessage();
            mailMessage.To.Add(new MailAddress(destinationMailAddress, personName));
            if (!String.IsNullOrEmpty(bcc) && bcc.IsValidEmailAddress())
            {
                mailMessage.Bcc.Add(bcc);
            }
            mailMessage.From = senderInfo;
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = subject;
            mailMessage.Body = body;

            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendMail(string destinationMailAddress, string bcc, string subject, string body)
        {
            var mailMessage = new MailMessage();
            mailMessage.To.Add(new MailAddress(destinationMailAddress));
            if (!String.IsNullOrEmpty(bcc) && bcc.IsValidEmailAddress())
            {
                mailMessage.Bcc.Add(bcc);
            }
            mailMessage.From = senderInfo;
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = subject;
            mailMessage.Body = body;

            await smtpClient.SendMailAsync(mailMessage);
        }

    }
}
