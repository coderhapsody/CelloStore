using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CelloStore.Data;

namespace CelloStore.ViewModels
{
    public class CheckOutViewModel
    {
        public CartViewModel Cart { get; set; }

        public bool SecretSenderName { get; set; }

        [Required]
        [Display(Name = "Recipient Name")]
        public string RecipientName { get; set; }

        [EmailAddress]
        [Display(Name = "Recipient Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string RecipientEmail { get; set; }

        [Required]
        [Display(Name = "Recipient zip code")]
        public string ZipCode { get; set; }


        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Recipient Location")]
        public int RecipientLocationTypeId { get; set; }
        public IEnumerable<AddressType> AddressTypes { get; set; }

        [Required]
        [Display(Name = "Recipient Address")]
        public string RecipientAddress { get; set; }

        [Required]
        [Display(Name = "Recipient State")]
        public string RecipientState { get; set; }

        [Required]
        [Display(Name = "Recipient City")]
        public string RecipientCity { get; set; }

        [Required]
        [Display(Name = "Recipient Phone Number")]
        public string RecipientPhoneNumber { get; set; }

        public bool CallRecipientFirst { get; set; }

        public string ToCardMessage { get; set; }
        public string FromCardMessage { get; set; }
        public string CardMessage { get; set; }

        public string Notes { get; set; }

        [Required]
        [Display(Name = "Payment Method")]
        public int PaymentMethodId { get; set; }
        public IEnumerable<PaymentMethod> PaymentMethods { get; set; }

        public string DeliveryArea { get; set; }
    }
}
