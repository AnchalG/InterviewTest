using System;
using WebApi.Interfaces;
using WebApi.Utilities;

namespace WebApi.BusinessLogic
{
    public class ExpensivePaymentGateway : IExpensivePaymentGateway
    {
        //Mocking purpose only. In read world, this iwll be an actuall Payment gateway
        public PaymentStatus ProceessAmount(decimal amount)
        {
            if (DateTime.Now.Second > 20 && DateTime.Now.Second < 40)
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
