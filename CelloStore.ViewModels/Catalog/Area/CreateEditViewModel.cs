using System.ComponentModel.DataAnnotations;

namespace CelloStore.ViewModels.Catalog.Area
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
