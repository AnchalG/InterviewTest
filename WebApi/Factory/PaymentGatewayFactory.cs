using WebApi.Interfaces;
using WebApi.Utilities;

namespace WebApi.Factory
{
    public class PaymentGatewayFactory: IPaymentGatewayFactory
    {
        private readonly ICheapPaymentGateway _cheapGateway;
        private readonly IExpensivePaymentGateway _expensiveGateway;
        private readonly IPremiumPaymentService _premiumGateway;

        public PaymentGatewayFactory(ICheapPaymentGateway cheapGateway, IExpensivePaymentGateway expensiveGateway, IPremiumPaymentService premiumGateway)
        {
            _cheapGateway = cheapGateway;
            _expensiveGateway = expensiveGateway;
            _premiumGateway = premiumGateway;
        }

        public PaymentStatus GetPaymentGateway(decimal amount)
        {
            PaymentStatus status;
            if (amount <= 20)
            {
                _cheapGateway.ProceessAmount(amount);
                status= PaymentStatus.Processed;

            }
            else if (amount > 20 && amount <= 500)
            {
                status = _expensiveGateway.ProceessAmount(amount);
                if (status == PaymentStatus.Failed)
                {
                    status = _cheapGateway.ProceessAmount(amount);
                }
            }
            else {

                int count = 1;
                do {
                    status= _premiumGateway.ProceessAmount(amount);
                    count++;

                }while (status != PaymentStatus.Processed && count <= 3);
            }

            return status;

        }

    }

    public interface IPaymentGatewayFactory {

        PaymentStatus GetPaymentGateway(decimal amount);
    }
}
