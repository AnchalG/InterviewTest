using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Model
{
    public class CreditCardDetails
    {
        [Key]
        [Required]
        public string CreditCardNumber { get; set; }

        [Required]
        public string CardHolder { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }//should be greater than todays date

        public string SecurityCode { get; set; }//not madatory
        [ForeignKey("CreditCardNo")]
        public ICollection<PaymentState> Payment { get; set; }
    }
}
