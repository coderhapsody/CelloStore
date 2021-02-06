using System;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.MailTemplate
{
    public class OrderMailViewModel : BaseViewModel
    {
        public string OrderNo { get; set; }        
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryTimeName { get; set; }
        public string CustomerName { get; set; }
        public string OrderStatusName { get; set; }
        public string CustomerMail { get; set; }
    }
}
