using System;
using Xunit;
using Moq;
using FluentAssertions;
using WebApi.Interfaces;
using WebApi.Controllers;
using WebApi.Model;
using WebApi.Utilities;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using WebApi.Factory;

namespace UnitTesting
{
    public class UnitTest_ProcessGateway
    {
        private Mock<IPaymentGatewayFactory> mockPaymentFactory;
        private Mock<IDataDal> mockDataDal;
        private Mock<IMapper> mockMapper;
        ProcessGateway sut;
        public UnitTest_ProcessGateway()
        {
            mockPaymentFactory = new Mock<IPaymentGatewayFactory>();
            mockMapper = new Mock<IMapper>();
            mockDataDal = new Mock<IDataDal>();
        }

        [InlineData(203, "6771798021000008", "XYX", "2022/10/11", "123", "3b8f30f5-8c6f-491b-955a-be2c06d1ffd6", "Processed")]
        [InlineData(203, "6771798021000008", "XYX", "2022/10/11", "123", "3b8f30f5-8c6f-491b-955a-be2c06d1ffd6", "Failed")]
        [Theory]
        public void TestProceessAmount_WithStatus(decimal amount, string creditCardNumber, string creditCardHolder, string expiryDate,
            string securityCode, string guid, string status)
        {
            PaymentModel model = new PaymentModel()
            {
                Amount = amount,
                CardHolder = creditCardHolder,
                CreditCardNumber = creditCardNumber,
                ExpirationDate = DateTime.Parse(expiryDate),
                SecurityCode = securityCode
            };

            CreditCardDetails creditCardDetails = new CreditCardDetails()
            {
                CardHolder = creditCardHolder,
                CreditCardNumber = creditCardNumber,
                ExpirationDate = DateTime.Parse(expiryDate),
                SecurityCode = securityCode
            };

            PaymentState paymentState = new PaymentState()
            {
                Amount = amount,
                CreditCardNo = creditCardNumber,
                StateOfTransation = status,
                TransactionId = Guid.Parse(guid)
            };

            mockMapper.Setup(x => x.Map<CreditCardDetails>(model)).Returns(creditCardDetails);

            mockDataDal.Setup(x => x.InsertCreditCardDetails(creditCardDetails));

            mockPaymentFactory.Setup(x => x.GetPaymentGateway(amount)).Returns(PaymentStatus.Processed);

            mockMapper.Setup(x => x.Map<PaymentState>(model)).Returns(paymentState);

            mockDataDal.Setup(x => x.InsertTransactionDetails(paymentState));

            sut = new ProcessGateway(mockPaymentFactory.Object, mockMapper.Object, mockDataDal.Object);
            
            var result = sut.ProceessAmount(model);

            Assert.NotNull(result);
            Assert.Equal(paymentState.Amount,result.Amount);
            Assert.Equal(paymentState.CreditCardNo,result.CreditCardNo);
            Assert.Equal(paymentState.TransactionId,result.TransactionId);
            Assert.Equal(paymentState.StateOfTransation,result.StateOfTransation);
        }




        [InlineData(203, "6771798021000008", "XYX", "2022/10/11", "123", "3b8f30f5-8c6f-491b-955a-be2c06d1ffd6", "Processed")]
        [InlineData(203, "6771798021000008", "XYX", "2022/10/11", "123", "3b8f30f5-8c6f-491b-955a-be2c06d1ffd6", "Failed")]
        [Theory]
        public void TestProceessAmount_VerifyMethodCalls(decimal amount, string creditCardNumber, string creditCardHolder, string expiryDate,
           string securityCode, string guid, string status)
        {
            //arrange
            PaymentModel model = new PaymentModel()
            {
                Amount = amount,
                CardHolder = creditCardHolder,
                CreditCardNumber = creditCardNumber,
                ExpirationDate = DateTime.Parse(expiryDate),
                SecurityCode = securityCode
            };

            CreditCardDetails creditCardDetails = new CreditCardDetails()
            {
                CardHolder = creditCardHolder,
                CreditCardNumber = creditCardNumber,
                ExpirationDate = DateTime.Parse(expiryDate),
                SecurityCode = securityCode
            };

            PaymentState paymentState = new PaymentState()
            {
                Amount = amount,
                CreditCardNo = creditCardNumber,
                StateOfTransation = status,
                TransactionId = Guid.Parse(guid)
            };

            mockMapper.Setup(x => x.Map<CreditCardDetails>(model)).Returns(creditCardDetails);

            mockDataDal.Setup(x => x.InsertCreditCardDetails(creditCardDetails));

            mockPaymentFactory.Setup(x => x.GetPaymentGateway(amount)).Returns(PaymentStatus.Processed);

            mockMapper.Setup(x => x.Map<PaymentState>(model)).Returns(paymentState);

            mockDataDal.Setup(x => x.InsertTransactionDetails(paymentState));

            //act

            sut = new ProcessGateway(mockPaymentFactory.Object, mockMapper.Object, mockDataDal.Object);

            sut.ProceessAmount(model);

            //Verify

            mockMapper.Verify(x => x.Map<CreditCardDetails>(model), Times.Once());

            mockDataDal.Verify(x => x.InsertCreditCardDetails(creditCardDetails), Times.Once());

            mockPaymentFactory.Verify(x => x.GetPaymentGateway(amount), Times.Once());

            mockMapper.Verify(x => x.Map<PaymentState>(model), Times.Once());

            mockDataDal.Verify(x => x.InsertTransactionDetails(paymentState), Times.Once());
        }



        [InlineData(203, "6771798021000008", "XYX", "2022/10/11", "123", "3b8f30f5-8c6f-491b-955a-be2c06d1ffd6", "Failed")]
        [Theory]
        public void TestProceessAmount_Exception(decimal amount, string creditCardNumber, string creditCardHolder, string expiryDate,
            string securityCode, string guid, string status)
        {
            PaymentModel model = new PaymentModel()
            {
                Amount = amount,
                CardHolder = creditCardHolder,
                CreditCardNumber = creditCardNumber,
                ExpirationDate = DateTime.Parse(expiryDate),
                SecurityCode = securityCode
            };

            CreditCardDetails creditCardDetails = new CreditCardDetails()
            {
                CardHolder = creditCardHolder,
                CreditCardNumber = creditCardNumber,
                ExpirationDate = DateTime.Parse(expiryDate),
                SecurityCode = securityCode
            };

            PaymentState paymentState = new PaymentState()
            {
                Amount = amount,
                CreditCardNo = creditCardNumber,
                StateOfTransation = status,
                TransactionId = Guid.Parse(guid)
            };

            mockMapper.Setup(x => x.Map<CreditCardDetails>(model)).Returns(creditCardDetails);

            mockDataDal.Setup(x => x.InsertCreditCardDetails(creditCardDetails)).Throws(new Exception());

            mockMapper.Setup(x => x.Map<PaymentState>(model)).Returns(paymentState);
            Assert.Equal(amount, paymentState.Amount);
            Assert.Equal(creditCardNumber,paymentState.CreditCardNo);
            Assert.Equal(Guid.Parse(guid), paymentState.TransactionId);
            Assert.Equal(status, paymentState.StateOfTransation);
        }

        [InlineData(203, "6771798021000008", "XYX", "2022/10/11", "123", "3b8f30f5-8c6f-491b-955a-be2c06d1ffd6", "Failed")]
        [Theory]
        public void TestProceessAmount_Exception2(decimal amount, string creditCardNumber, string creditCardHolder, string expiryDate,
            string securityCode, string guid, string status)
        {
            PaymentModel model = new PaymentModel()
            {
                Amount = amount,
                CardHolder = creditCardHolder,
                CreditCardNumber = creditCardNumber,
                ExpirationDate = DateTime.Parse(expiryDate),
                SecurityCode = securityCode
            };

            CreditCardDetails creditCardDetails = new CreditCardDetails()
            {
                CardHolder = creditCardHolder,
                CreditCardNumber = creditCardNumber,
                ExpirationDate = DateTime.Parse(expiryDate),
                SecurityCode = securityCode
            };

            PaymentState paymentState = new PaymentState()
            {
                Amount = amount,
                CreditCardNo = creditCardNumber,
                StateOfTransation = status,
                TransactionId = Guid.Parse(guid)
            };

            mockMapper.Setup(x => x.Map<CreditCardDetails>(model)).Returns(creditCardDetails);

            mockDataDal.Setup(x => x.InsertCreditCardDetails(creditCardDetails));

            mockPaymentFactory.Setup(x => x.GetPaymentGateway(amount)).Returns(PaymentStatus.Processed);

            mockMapper.Setup(x => x.Map<PaymentState>(model)).Returns(paymentState);

            mockDataDal.Setup(x => x.InsertTransactionDetails(paymentState)).Throws(new Exception());

            mockMapper.Setup(x => x.Map<PaymentState>(model)).Returns(paymentState);
            Assert.Equal(amount, paymentState.Amount);
            Assert.Equal(creditCardNumber, paymentState.CreditCardNo);
            Assert.Equal(Guid.Parse(guid), paymentState.TransactionId);
            Assert.Equal(status, paymentState.StateOfTransation);
        }


    }
}
