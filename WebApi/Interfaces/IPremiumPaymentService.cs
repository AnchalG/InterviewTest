using WebApi.Utilities;

namespace WebApi.Interfaces
{
    public interface IPremiumPaymentService
    {
        PaymentStatus ProceessAmount(decimal amount);
    }
}
