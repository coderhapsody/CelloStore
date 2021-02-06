using System.Collections.Generic;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Catalog.Product
{
    public class IndexViewModel : BasePageableViewModel
    {
        public string Search { get; set; }
        public IEnumerable<ListProductViewModel> List { get; set; }        
    }
}
