using System.Collections.Generic;
using CelloStore.Data;
using CelloStore.ViewModels.Sales.Order;

namespace CelloStore.ViewModels
{
    public class OrderHistoryDetailViewModel
    {
        public Order SelectedOrder { get; set; }
        public IEnumerable<OrderDetail> SelectedOrderDetails { get; set; }
        public IEnumerable<OrderStatusHistoryViewModel> StatusHistories { get; set; }
        public PaymentConfirmation PaymentConfirmation { get; set; }
        public string DeliveryTimeName { get; set; }
    }
}
