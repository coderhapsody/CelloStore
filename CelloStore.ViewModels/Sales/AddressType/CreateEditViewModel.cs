using System.ComponentModel.DataAnnotations;

namespace CelloStore.ViewModels.Sales.AddressType
{
    public class CreateEditViewModel
    {        
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]        
        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
