using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Model
{
    public class PaymentState
    {
        [Key]
        [Required]
        public Guid TransactionId { get; set; }
        [Required]
        public string StateOfTransation { get; set; }

        [Required]
        public string CreditCardNo { get; set; }
        public decimal Amount { get; set; }

    }

  
}
