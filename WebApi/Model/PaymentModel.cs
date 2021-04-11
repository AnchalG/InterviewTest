using System;
using System.ComponentModel.DataAnnotations;
using WebApi.Utilities;

namespace WebApi.Model
{
    public class PaymentModel
    {
        [Required]
        [Key]
        [CreditCardValidate(ErrorMessage ="Credit Card Number is incorrect")]
        public string CreditCardNumber { get; set; }
        
        [Required]
        public string CardHolder { get; set; }
        
        [Required]
        [DateValidateAttribute(ErrorMessage ="ExpirationDate should be greater than today's Date")]
        public DateTime ExpirationDate { get; set; }//should be greater than todays date
        
        [StringLength(3, MinimumLength = 3)]

        public string SecurityCode { get; set; }//not madatory
       
        [Required]
        [AmountValidation(ErrorMessage ="Amount should be a positive number")]
        public decimal Amount { get; set; }
    }
}
