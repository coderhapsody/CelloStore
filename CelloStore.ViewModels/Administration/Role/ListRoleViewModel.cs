using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Administration.Role
{
    public class ListRoleViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSystemUsed { get; set; }
    }
}
