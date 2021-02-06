using System.ComponentModel.DataAnnotations;

namespace CelloStore.ViewModels.Administration.Role
{
    public class CreateEditViewModel
    {        
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]        
        public string Name { get; set; }

        [Display(Name="Used by System")]
        public bool IsSystemUsed { get; set; }
    }
}
