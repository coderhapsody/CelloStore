using System;
using System.Collections.Generic;
using CelloStore.ViewModels.Base;
using CelloStore.ViewModels.Sales.Order;

namespace CelloStore.ViewModels
{
    public class OrderHistoryViewModel : BaseViewModel
    {
        public IEnumerable<ListOrderViewModel> Orders { get; set; }
        public int PersonId { get; set; }
    }
}
