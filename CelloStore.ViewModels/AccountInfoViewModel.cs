using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels
{
    public class AccountInfoViewModel : BaseViewModel
    {
        public string AccountName { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool IsAdministrator { get; set; }
    }
}
