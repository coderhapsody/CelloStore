using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Catalog.Category
{
    public class CreateEditViewModel : BaseRowDataViewModel
    {        
        [Required]
        [MaxLength(50)]        
        public string Name { get; set; }

        public IEnumerable<CelloStore.Data.Category> ParentCategories { get; set; }

        [Display(Name="Parent Category")]
        public int? ParentCategoryId { get; set; }

        [Display(Name="Is Active")]
        public bool IsActive { get; set; }
    }
}
