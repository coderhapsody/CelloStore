using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels
{
    public class OrderDetailViewModel : BaseViewModel
    {
        public int LineNo { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscValue { get; set; }
    }
}
