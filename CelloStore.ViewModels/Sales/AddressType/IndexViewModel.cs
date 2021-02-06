using System.Collections.Generic;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Sales.AddressType
{
    public class IndexViewModel : BasePageableViewModel
    {
        public IEnumerable<ListAddressTypeViewModel> List { get; set; }        
    }
}
