using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Sales.AddressType
{
    public class ListAddressTypeViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IsActive { get; set; }
    }
}
