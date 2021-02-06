using System.ComponentModel.DataAnnotations;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Administration.System
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        [Required]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
