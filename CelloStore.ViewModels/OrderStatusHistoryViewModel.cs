using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelloStore.ViewModels
{
    public class OrderStatusHistoryViewModel
    {
        public DateTime CreatedDate { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public bool SendMail { get; set; }
    }
}
