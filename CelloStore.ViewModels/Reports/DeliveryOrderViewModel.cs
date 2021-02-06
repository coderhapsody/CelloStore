using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Reports
{
    public class DeliveryOrderViewModel : BaseViewModel
    {
        public string DeliveryOrderNo { get; set; }
        public string RecipientName { get; set; }
        public string DeliveryAddress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DeliveryDateTime { get; set; }        
        public string OrderNo { get; set; }
        public string DeliveredBy { get; set; }
        public string AcceptedBy { get; set; }
        public string Notes { get; set; }        
        public IList<DeliveryOrderDetailViewModel> Details { get; set; }
    }
}
