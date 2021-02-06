using System.Collections.Generic;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Sales.OrderStatus
{
    public class IndexViewModel : BasePageableViewModel
    {
        public IEnumerable<Data.OrderStatus> List { get; set; }        
    }
}
