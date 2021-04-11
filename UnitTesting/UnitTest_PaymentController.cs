using System;
using Xunit;
using Moq;
using FluentAssertions;
using WebApi.Interfaces;
using WebApi.Controllers;
using WebApi.Model;
using WebApi.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace UnitTesting
{
    public class UnitTest_PaymentController
    {
        [InlineData("6771798021000008",true)]
        [InlineData("2222405343248877",false)]
        [Theory]
        public void ValidateCreditCardNumber(string creditCardNo, bool expectedResult)
        {
            var attribute = new CreditCardValidateAttribute();
            var result = attribute.IsValid(creditCardNo);
            Assert.Equal(expectedResult, result);
        }

        [InlineData("2022/10/11", true)]
        [InlineData("2021/01/31", false)]
        [Theory]
        public void ValidateExpiration(string expirationDate, bool expected)
        {
            var attribute = new DateValidateAttribute();
            var result = attribute.IsValid(DateTime.Parse(expirationDate));
            Assert.Equal(expected, result);
        }

        [InlineData(203, true)]
        [InlineData(-3, false)]
        [Theory]
        public void ValidateAmount(decimal amount, bool expected)
        {
            var attribute = new AmountValidationAttribute();
            var result = attribute.IsValid(amount);
            Assert.Equal(expected, result);
        }


        [InlineData(203,"6771798021000008","XYX", "2022/10/11","123", "3b8f30f5-8c6f-491b-955a-be2c06d1ffd6","Processed",200, "Transaltion Id: 3b8f30f5-8c6f-491b-955a-be2c06d1ffd6 was completed successfully.")]
        [InlineData(203,"6771798021000008","XYX", "2022/10/11","123", "3b8f30f5-8c6f-491b-955a-be2c06d1ffd6","Failed",500, "Transaltion Id: 3b8f30f5-8c6f-491b-955a-be2c06d1ffd6 failed. Please try again later.")]
        [Theory]
        public void TestProcessPayment_WithStatus(decimal amount, string creditCardNumber, string creditCardHolder, string expiryDate,
            string securityCode, string guid,string status,int statuscode,string outputString) {
            var mockProcessGateway = new Mock<IProcessGateway>();

            PaymentModel model = new PaymentModel() {Amount=amount,CardHolder=creditCardHolder,CreditCardNumber=creditCardNumber,
                ExpirationDate= DateTime.Parse(expiryDate),SecurityCode=securityCode};

            PaymentState paymentState = new PaymentState() { Amount = amount, CreditCardNo = creditCardNumber, StateOfTransation = status, 
                TransactionId = Guid.Parse(guid) };

            mockProcessGateway.Setup(x => x.ProceessAmount(model)).Returns(paymentState);

            var sut = new PaymentController(mockProcessGateway.Object);

            var result = sut.ProcessPayment(model);

            var okResult = result as ObjectResult;

            Assert.NotNull(okResult);
            Assert.Equal(statuscode, okResult.StatusCode);
            Assert.Equal(outputString, okResult.Value);


        }

        [InlineData(203, "6771798021000008", "XYX", "2022/10/11", "123", "3b8f30f5-8c6f-491b-955a-be2c06d1ffd6", "", 500, "Something went wrong. Please try again later.")]
        [Theory]
        public void TestProcessPayment_WithNoStatus(decimal amount, string creditCardNumber, string creditCardHolder, string expiryDate,
           string securityCode, string guid, string status, int statuscode, string outputString)
        {
            var mockProcessGateway = new Mock<IProcessGateway>();

            PaymentModel model = new PaymentModel()
            {
                Amount = amount,
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

            mockProcessGateway.Setup(x => x.ProceessAmount(model)).Returns(paymentState);

            var sut = new PaymentController(mockProcessGateway.Object);

            var result = sut.ProcessPayment(model);

            var okResult = result as ObjectResult;
            var errorString = okResult.Value as ObjectResult;

            Assert.NotNull(okResult);
            Assert.Equal(statuscode, okResult.StatusCode);
            Assert.Equal(outputString, errorString.Value);


        }


        [InlineData(203, "6771798021000008", "XYX", "2022/10/11", "123", "3b8f30f5-8c6f-491b-955a-be2c06d1ffd6", "Processed")]
        [InlineData(203, "6771798021000008", "XYX", "2022/10/11", "123", "3b8f30f5-8c6f-491b-955a-be2c06d1ffd6", "Failed")]
        [InlineData(203, "6771798021000008", "XYX", "2022/10/11", "123", "3b8f30f5-8c6f-491b-955a-be2c06d1ffd6", "")]
        [Theory]
        public void TestProcessPayment_CheckMethodCalls(decimal amount, string creditCardNumber, string creditCardHolder, string expiryDate,
          string securityCode, string guid, string status)
        {
            var mockProcessGateway = new Mock<IProcessGateway>();

            PaymentModel model = new PaymentModel()
            {
                Amount = amount,
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

            mockProcessGateway.Setup(x => x.ProceessAmount(model)).Returns(paymentState);

            var sut = new PaymentController(mockProcessGateway.Object);

            sut.ProcessPayment(model);

            mockProcessGateway.Verify(x => x.ProceessAmount(model), Times.Once);


        }

        [Fact]
        public void TestProcessPayment_TextException()
        {
            var mockProcessGateway = new Mock<IProcessGateway>();

            PaymentModel model = new PaymentModel();

            PaymentState paymentState = new PaymentState();

            mockProcessGateway.Setup(x => x.ProceessAmount(model)).Throws(new Exception("Something went wrong. Please try again later."));

            var sut = new PaymentController(mockProcessGateway.Object);

            var result = sut.ProcessPayment(model);
            var okResult = result as ObjectResult;
            var errorString = okResult.Value as ObjectResult;

            Assert.NotNull(okResult);
            Assert.Equal(500, okResult.StatusCode);
            Assert.Equal("Something went wrong. Please try again later.",errorString.Value);
        }

    }
}
