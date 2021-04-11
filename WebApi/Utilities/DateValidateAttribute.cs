using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Utilities
{
    public class DateValidateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime credtiCardExpiryDate = (DateTime)value;

            if (credtiCardExpiryDate.CompareTo(DateTime.Now) > 0)
            {
                return true;
            }
            else return false;
        }

    }

}
