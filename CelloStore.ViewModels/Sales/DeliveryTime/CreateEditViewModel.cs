using System.ComponentModel.DataAnnotations;

namespace CelloStore.ViewModels.Sales.DeliveryTime
{
    public class CreateEditViewModel
    {        
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]        
        public string Name { get; set; }

        [Display(Name="Is Active")]
        public bool IsActive { get; set; }
    }
}
