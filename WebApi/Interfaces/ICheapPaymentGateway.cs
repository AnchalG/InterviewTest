
using WebApi.Utilities;

namespace WebApi.Interfaces
{
    public interface ICheapPaymentGateway
    {
        PaymentStatus ProceessAmount(decimal amount);
    }
}
