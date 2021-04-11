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
    public class UnitTest_PaymentGatewayFactory
    {
        private Mock<ICheapPaymentGateway> mockCheapGateway;
        private Mock<IExpensivePaymentGateway> mockExpensiveGateway;
        private Mock<IPremiumPaymentService> mockPremiumGateway;
        PaymentGatewayFactory sut;
        public UnitTest_PaymentGatewayFactory()
        {
            mockCheapGateway = new Mock<ICheapPaymentGateway>();
            mockExpensiveGateway = new Mock<IExpensivePaymentGateway>();
            mockPremiumGateway = new Mock<IPremiumPaymentService>();
        }

        [InlineData(203, "Processed")]
        [InlineData(203,"Failed")]
        [InlineData(2, "Processed")]
        [InlineData(502, "Processed")]
        [InlineData(502, "Failed")]
        [Theory]
        public void GetPaymentGateway_WithStatus(decimal amount,string status)
        {
            PaymentStatus expected;
            Enum.TryParse<PaymentStatus>(status, out expected);

            mockCheapGateway.Setup(x => x.ProceessAmount(amount)).Returns(expected);
            mockExpensiveGateway.Setup(x => x.ProceessAmount(amount)).Returns(expected);
            mockPremiumGateway.Setup(x => x.ProceessAmount(amount)).Returns(expected);

            sut = new PaymentGatewayFactory(mockCheapGateway.Object, mockExpensiveGateway.Object, mockPremiumGateway.Object);
            
            var result = sut.GetPaymentGateway(amount);

            Assert.Equal(expected,result);
        }

        [InlineData(203, "Processed")]
        [Theory]
        public void GetPaymentGateway_VerifyMethodCallOfExpensiveGateway(decimal amount, string status)
        {
            PaymentStatus expected;
            Enum.TryParse<PaymentStatus>(status, out expected);

            mockCheapGateway.Setup(x => x.ProceessAmount(amount)).Returns(expected);
            mockExpensiveGateway.Setup(x => x.ProceessAmount(amount)).Returns(expected);
            mockPremiumGateway.Setup(x => x.ProceessAmount(amount)).Returns(expected);

            sut = new PaymentGatewayFactory(mockCheapGateway.Object, mockExpensiveGateway.Object, mockPremiumGateway.Object);

            sut.GetPaymentGateway(amount);

            mockExpensiveGateway.Verify(x => x.ProceessAmount(amount), Times.Once);
           
        }

        [InlineData(203, "Failed")]
        [Theory]
        public void GetPaymentGateway_VerifyMethodCallOf2GateWay(decimal amount, string status)
        {
            PaymentStatus expected;
            Enum.TryParse<PaymentStatus>(status, out expected);

            mockCheapGateway.Setup(x => x.ProceessAmount(amount)).Returns(expected);
            mockExpensiveGateway.Setup(x => x.ProceessAmount(amount)).Returns(expected);
            mockPremiumGateway.Setup(x => x.ProceessAmount(amount)).Returns(expected);

            sut = new PaymentGatewayFactory(mockCheapGateway.Object, mockExpensiveGateway.Object, mockPremiumGateway.Object);

            sut.GetPaymentGateway(amount);

            mockExpensiveGateway.Verify(x => x.ProceessAmount(amount), Times.Once);
            mockCheapGateway.Verify(x => x.ProceessAmount(amount), Times.Once);

        }


        [Theory]
        [InlineData(2, "Processed")]
        public void GetPaymentGateway_VerifyMethodCallOfCheapGateway(decimal amount, string status)
        {
            PaymentStatus expected;
            Enum.TryParse<PaymentStatus>(status, out expected);

            mockCheapGateway.Setup(x => x.ProceessAmount(amount)).Returns(expected);
            mockExpensiveGateway.Setup(x => x.ProceessAmount(amount)).Returns(expected);
            mockPremiumGateway.Setup(x => x.ProceessAmount(amount)).Returns(expected);

            sut = new PaymentGatewayFactory(mockCheapGateway.Object, mockExpensiveGateway.Object, mockPremiumGateway.Object);

            sut.GetPaymentGateway(amount);

            mockCheapGateway.Verify(x => x.ProceessAmount(amount), Times.Once);

        }



        [InlineData(502, "Processed")]
        [InlineData(502, "Failed")]
        [Theory]
        public void GetPaymentGateway_VerifyMethodCallOfPremiumService(decimal amount, string status)
        {
            PaymentStatus expected;
            Enum.TryParse<PaymentStatus>(status, out expected);

            mockCheapGateway.Setup(x => x.ProceessAmount(amount)).Returns(expected);
            mockExpensiveGateway.Setup(x => x.ProceessAmount(amount)).Returns(expected);
            mockPremiumGateway.Setup(x => x.ProceessAmount(amount)).Returns(expected);

            sut = new PaymentGatewayFactory(mockCheapGateway.Object, mockExpensiveGateway.Object, mockPremiumGateway.Object);

            sut.GetPaymentGateway(amount);

            mockPremiumGateway.Verify(x => x.ProceessAmount(amount), Times.AtLeastOnce);

        }

    }
}
