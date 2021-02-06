using System;
using System.ComponentModel.DataAnnotations;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {
        [Required]
        [Display(Name="First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        [Display(Name = "PostalCode")]
        public string PostalCode { get; set; }

        public char? Gender { get; set; }

        [Display(Name = "Birthday")]
        public DateTime? BirthDate { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password field is required")]
        [Compare("Password", ErrorMessage = "Confirm Password does not match with entered password")]
        public string ConfirmPassword { get; set; }

        public string ChallangeCode { get; set; }
        public string ActivationUrl { get; set; }
    }
}
