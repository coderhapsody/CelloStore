using System.Collections.Generic;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Catalog.Category
{
    public class IndexViewModel : BasePageableViewModel
    {
        public IEnumerable<ListCategoryViewModel> List { get; set; }        
    }
}
