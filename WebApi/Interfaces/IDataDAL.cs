using WebApi.Model;

namespace WebApi.Interfaces
{
    public interface IDataDal
    {
        void InsertCreditCardDetails(CreditCardDetails creditCardDetails);
        void InsertTransactionDetails(PaymentState paymentState);

    }
}
