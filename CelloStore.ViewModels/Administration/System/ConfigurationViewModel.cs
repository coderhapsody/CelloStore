using System.ComponentModel.DataAnnotations;

namespace CelloStore.ViewModels.Administration.System
{
    public class ConfigurationViewModel
    {
        [Required]
        public string SmtpServer { get; set; }

        public short SmtpPort { get; set; }
        public bool SmtpAuthentication { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set; }
        public bool SmtpSsl { get; set; }

        [Required]
        public string SystemSenderAddress { get; set; }
        [Required]
        public string SystemSenderName { get; set; }
    }
}
