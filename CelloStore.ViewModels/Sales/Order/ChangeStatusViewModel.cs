using System;
using System.Linq;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Sales.Order
{
    public class ChangeStatusViewModel : BaseViewModel
    {
        public int  OrderId { get; set; }
        public int CurrentStatusId { get; set; }
        public string CurrentStatus { get; set; }
        public string Notes { get; set; }
        public int NewStatusId { get; set; }
        public bool SendMail { get; set; }
    }
}
