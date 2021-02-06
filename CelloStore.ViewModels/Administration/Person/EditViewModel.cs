using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Administration.Person
{
    public class EditViewModel : RegistrationViewModel
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public DateTime LastChangePassword { get; set; }
        public DateTime LastLogOn { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }
        public string LastIpAddress { get; set; }

    }
}
