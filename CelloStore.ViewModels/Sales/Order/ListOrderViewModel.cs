using System;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Sales.Order
{
    public class ListOrderViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string PersonName { get; set; }
        public string OrderStatus { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int DeliveryTime { get; set; }
        public string DeliveryTimeName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
