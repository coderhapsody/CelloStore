using System.Collections.Generic;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Administration.Role
{
    public class IndexViewModel : BaseViewModel
    {
        public IList<ListRoleViewModel> List { get; set; }
    }
}
