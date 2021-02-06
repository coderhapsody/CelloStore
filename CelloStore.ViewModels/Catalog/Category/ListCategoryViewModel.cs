using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Catalog.Category
{
    public class ListCategoryViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        public string ParentCategory { get; set; }
        public bool IsActive { get; set; }
    }
}
