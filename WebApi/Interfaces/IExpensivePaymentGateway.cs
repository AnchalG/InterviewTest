using WebApi.Utilities;

namespace WebApi.Interfaces
{
    public interface IExpensivePaymentGateway
    {
        PaymentStatus ProceessAmount(decimal amount);
    }
}
