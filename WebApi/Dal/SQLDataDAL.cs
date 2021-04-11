using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Interfaces;
using WebApi.Model;

namespace WebApi.Dal
{
    public class SqlDataDal : IDataDal
    {
        private readonly AppDBContext _appDBContext;
        public SqlDataDal(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        public void InsertCreditCardDetails(CreditCardDetails creditCardDetails)
        {
            try
            {
                var entity = _appDBContext.CreditCardDetails.FirstOrDefault(x => x.CreditCardNumber == creditCardDetails.CreditCardNumber);
                if (entity == null)
                {
                    _appDBContext.CreditCardDetails.Add(creditCardDetails);
                    _appDBContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                //log erro
            }

        }

        public void InsertTransactionDetails(PaymentState paymentState)
        {
            try
            {
                _appDBContext.PaymentState.Add(paymentState);
                _appDBContext.SaveChanges();
            }
            catch (Exception ex)
            {
                //log erro
            }
        }
    }
}
