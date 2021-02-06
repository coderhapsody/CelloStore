using System.Collections.Generic;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Sales.DeliveryTime
{
    public class IndexViewModel : BasePageableViewModel
    {
        public IEnumerable<ListDeliveryTimeViewModel> List { get; set; }        
    }
}
