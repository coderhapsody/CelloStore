using System;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Administration.Person
{
    public class ListPersonViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? LastLogOn { get; set; }
        public DateTime? LastChangePassword { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set;  }
    }
}
