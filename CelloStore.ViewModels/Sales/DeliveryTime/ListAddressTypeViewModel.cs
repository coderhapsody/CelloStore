using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Sales.DeliveryTime
{
    public class ListDeliveryTimeViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IsActive { get; set; }
    }
}
