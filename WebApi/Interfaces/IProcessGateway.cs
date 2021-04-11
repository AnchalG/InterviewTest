using WebApi.Model;

namespace WebApi.Interfaces
{
    public interface IProcessGateway
    {
        PaymentState ProceessAmount(PaymentModel payment);
    }
   
}
