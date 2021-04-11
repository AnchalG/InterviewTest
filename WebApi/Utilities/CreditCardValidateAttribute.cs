using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Utilities
{

    //[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CreditCardValidateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                string creditCardNumber = (string)value;
                long number;
                if (long.TryParse(creditCardNumber, out number))
                {

                    return (getSize(number) >= 13 &&
                getSize(number) <= 16) &&
                (prefixMatched(number, 4) ||
                prefixMatched(number, 5) ||
                prefixMatched(number, 37) ||
                prefixMatched(number, 6)) &&
                ((sumOfDoubleEvenPlace(number) +
                sumOfOddPlace(number)) % 10 == 0);
                }
                else return false;
            }
            return false;
        }

        public int sumOfDoubleEvenPlace(long number)
        {
            int sum = 0;
            String num = number + "";
            for (int i = getSize(number) - 2; i >= 0; i -= 2)
                sum += getDigit(int.Parse(num[i] + "") * 2);

            return sum;
        }

        // Return this number if it is a
        // single digit, otherwise, return
        // the sum of the two digits
        public int getDigit(int number)
        {
            if (number < 9)
                return number;
            return number / 10 + number % 10;
        }

        // Return sum of odd-place digits in number
        public int sumOfOddPlace(long number)
        {
            int sum = 0;
            String num = number + "";
            for (int i = getSize(number) - 1; i >= 0; i -= 2)
                sum += int.Parse(num[i] + "");
            return sum;
        }

        // Return true if the digit d
        // is a prefix for number
        public bool prefixMatched(long number, int d)
        {
            return getPrefix(number, getSize(d)) == d;
        }

        // Return the number of digits in d
        public int getSize(long d)
        {
            String num = d + "";
            return num.Length;
        }

        // Return the first k number of digits from
        // number. If the number of digits in number
        // is less than k, return number.
        public long getPrefix(long number, int k)
        {
            if (getSize(number) > k)
            {
                String num = number + "";
                return long.Parse(num.Substring(0, k));
            }
            return number;
        }


    }


}
