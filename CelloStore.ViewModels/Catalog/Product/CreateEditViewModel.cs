using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CelloStore.ViewModels.Base;
using CelloStore.ViewModels.Common;

namespace CelloStore.ViewModels.Catalog.Product
{
    public class CreateEditViewModel : BaseRowDataViewModel
    {        
        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        [Required]
        [MaxLength(50)]        
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        public IEnumerable<Data.Category> Categories { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public string Photo { get; set; }
        public long Size { get; set; }

        public IEnumerable<Data.Area> Areas { get; set; }
        public IEnumerable<Data.Tag> Tags { get; set; }

        public string Ext { get; set; }

        [Display(Name = "Featured Product")]
        public bool IsFeatured { get; set; }
    }
}
