using System;
using System.ComponentModel.DataAnnotations;

namespace CelloStore.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
