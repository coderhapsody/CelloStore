using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelloStore.ViewModels.Reports
{
    public class KuitansiViewModel
    {
        public string ReceivedFrom { get; set; }
        public decimal Amount { get; set; }
        public string ProductName { get; set; }
        public string InvoiceNo { get; set; }
        public string Terbilang { get; set; }
        public DateTime InvoiceDate { get; set; }
    }
}
