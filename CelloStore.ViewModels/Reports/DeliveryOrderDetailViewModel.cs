using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Reports
{
    public class DeliveryOrderDetailViewModel : BaseViewModel
    {
        public string ProductNo { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
    }
}
