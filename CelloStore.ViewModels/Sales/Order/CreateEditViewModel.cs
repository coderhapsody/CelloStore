using System.Collections.Generic;

namespace CelloStore.ViewModels.Sales.Order
{
    public class CreateEditViewModel
    {        
        public CelloStore.Data.Order SelectedOrder { get; set; }
        public IEnumerable<CelloStore.Data.OrderDetail> SelectedOrderDetails { get; set; }
        public IEnumerable<OrderStatusHistoryViewModel> StatusHistories { get; set; }
        public CelloStore.Data.PaymentConfirmation PaymentConfirmation { get; set; }
    }
}
