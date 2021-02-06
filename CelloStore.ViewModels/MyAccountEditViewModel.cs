using System;
using System.ComponentModel.DataAnnotations;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels
{
    public class MyAccountEditViewModel : BaseViewModel
    {
        [Required]
        [Display(Name = "First Name")]
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

        public char Gender { get; set; }

        [Display(Name = "Birthday")]
        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
