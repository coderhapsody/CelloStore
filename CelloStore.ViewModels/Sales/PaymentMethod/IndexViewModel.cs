using System.Collections.Generic;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Sales.PaymentMethod
{
    public class IndexViewModel : BasePageableViewModel
    {
        public string Search { get; set; }
        public IEnumerable<CelloStore.Data.PaymentMethod> List { get; set; }        
    }
}
