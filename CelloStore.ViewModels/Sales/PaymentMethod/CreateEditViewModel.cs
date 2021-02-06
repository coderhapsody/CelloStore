using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CelloStore.ViewModels.Sales.PaymentMethod
{
    public class CreateEditViewModel
    {        
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]        
        public string Name { get; set; }

        [AllowHtml]
        public string Notes { get; set; }
    }
}
