using System.ComponentModel.DataAnnotations;

namespace CelloStore.ViewModels.Catalog.Tag
{
    public class CreateEditViewModel
    {        
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]        
        public string Name { get; set; }
    }
}
