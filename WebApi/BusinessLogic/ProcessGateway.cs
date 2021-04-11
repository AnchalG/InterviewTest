using System;
using WebApi.Factory;
using WebApi.Interfaces;
using AutoMapper;
using WebApi.Utilities;

namespace WebApi.Model
{
    public class ProcessGateway : IProcessGateway
    {
        private readonly IPaymentGatewayFactory _factory;
        private readonly IMapper _mapper;
        private readonly IDataDal _dal;
        public ProcessGateway(IPaymentGatewayFactory factory, IMapper mapper, IDataDal dal) {

            _factory = factory;
            _mapper = mapper;
            _dal = dal;
        }
        public PaymentState ProceessAmount(PaymentModel payment)
        {
            PaymentState paymentState;
            try
            {

                CreditCardDetails creditCardDetails = _mapper.Map<CreditCardDetails>(payment);

                _dal.InsertCreditCardDetails(creditCardDetails);
                
                var status = _factory.GetPaymentGateway(payment.Amount);

                paymentState = _mapper.Map<PaymentState>(payment);

                paymentState.StateOfTransation = status.ToString();

                _dal.InsertTransactionDetails(paymentState);

            }
            catch (Exception ex) {
                paymentState = _mapper.Map<PaymentState>(payment);
                paymentState.StateOfTransation = PaymentStatus.Failed.ToString();

            }

            return paymentState;
        }
    }
}
