using System.ComponentModel.DataAnnotations;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Page.Article
{
    public class CreateEditViewModel : BaseRowDataViewModel
    {                
        [Required]
        public string UrlToken { get; set; }

        [Required]
        public string Subject { get; set; }

        public string Content { get; set; }

        public bool IsPublished { get; set; }
        
    }
}
