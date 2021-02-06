using System;
using System.Collections.Generic;
using System.Linq;

namespace CelloStore.ViewModels
{
    public class CartViewModel
    {
        public string UserName { get; set; }
        public IList<OrderDetailViewModel> OrderDetails { get; set; }

        public DateTime DeliveryDate { get; set; }
        public byte DeliveryTime { get; set; }
        public string DeliveryTimeName { get; set; }
        public string VoucherCode { get; set; }
        public int OrderAreaId { get; set; }

        public decimal GrandTotal
        {
            get { return OrderDetails.Sum(ord => ord.Qty*ord.UnitPrice - ord.DiscValue); }
        }
    }
}
