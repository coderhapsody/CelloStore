using System.ComponentModel.DataAnnotations;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Page.EmailTemplate
{
    public class EditViewModel : BaseViewModel
    {
        public int Id { get; set; }        
        public string Name { get; set; }

        public string Bcc { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        public string Tokens { get; set; }
    }
}
