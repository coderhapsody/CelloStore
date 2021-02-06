using System.Collections.Generic;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Sales.Order
{
    public class IndexViewModel : BasePageableViewModel
    {
        public string Search { get; set; }
        public IEnumerable<ListOrderViewModel> List { get; set; }

        
    }
}
