using System.ComponentModel.DataAnnotations;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Sales.OrderStatus
{
    public class EditViewModel : BaseViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Name { get; set; }

        [Display(Name = "Email Template")]
        public string EmailTemplate { get; set; }

        public bool SendMail { get; set; }
    }
}
