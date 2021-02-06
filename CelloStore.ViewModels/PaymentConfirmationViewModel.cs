using System;
using System.ComponentModel.DataAnnotations;

namespace CelloStore.ViewModels
{
    public class PaymentConfirmationViewModel
    {
        [Required]
        [Display(Name = "Order Number")]
        public string OrderNo { get; set; }
        
        [Required]
        [Display(Name = "Sender Name")]
        public string SenderName { get; set; }
        
        [Required]
        [Display(Name = "Bank")]
        public int BankId { get; set; }

        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }
        
        [Required]
        [Display(Name = "Payment Amount")]
        public decimal PayAmount { get; set; }

        public string Notes { get; set; }

        [Required]
        [Display(Name = "Payment Method")]
        public int PaymentMethodId { get; set; }
    }
}
