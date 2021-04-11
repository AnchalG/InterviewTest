using WebApi.Interfaces;
using WebApi.Utilities;

namespace WebApi.BusinessLogic
{
    public class CheapPaymentGateway : ICheapPaymentGateway
    {
        //Mocking purpose only. In read world, this iwll be an actuall Payment gateway
        public PaymentStatus ProceessAmount(decimal amount)
        {
            return PaymentStatus.Processed;
        }
    }
}
