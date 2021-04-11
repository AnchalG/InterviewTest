using System;
using WebApi.Interfaces;
using WebApi.Utilities;

namespace WebApi.BusinessLogic
{
    public class PremiumPaymentService : IPremiumPaymentService
    {//Mocking purpose only. In read world, this iwll be an actuall Payment gateway
        public PaymentStatus ProceessAmount(decimal amount)
        {

            Random random = new Random();
            int randomNumber= random.Next(1, 60);

            if (randomNumber > 20 && randomNumber < 40)
            {
                return PaymentStatus.Failed;

            }
            else
            {
                return PaymentStatus.Processed;
            }
        }
    }
}
