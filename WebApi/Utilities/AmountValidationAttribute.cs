using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Utilities
{
    public class AmountValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            decimal input = (decimal)value;
            if (input > 0 && input <= decimal.MaxValue) {
                return true;
            
            }
            else return false;
        }

    }

}
