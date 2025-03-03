using FluentAssertions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Models;
using Hot.Domain.Enums;
using Hot.Nyaradzo.Domain.Entities;
using Hot.Nyaradzo.Infrastructure.Services;
using Hot.Tests.Common.Fakers.DbContext;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hot.Nyaradzo.Test.Services
{
    public class NyaradzoServiceTests
    {
        [Fact]
        public async Task ProcessPayment_ShouldReturnNyaradzoResultModel_Test()
        {
            //Arrange
            var paymentResult = DbContextFaker.GenerateNyaradzoPaymentResultTestData();
            var service = Substitute.For<IAPIService>();
            service.Post<Hot.Application.Common.Models.PaymentResult, PaymentRequest>(Arg.Any<string>(), Arg.Any<PaymentRequest>()).Returns(paymentResult);
            
            var sut = new NyaradzoService(service);
            //Act
            var result = await sut.ProcessPaymentAsync("12356N", "Reference", 500, 500, 1, DateTime.Now , Currency.USD );

            //Assert
            result.Should().NotBe(null);
            result.ValidResponse.Should().BeTrue();
            await service.Received().Post<Hot.Application.Common.Models.PaymentResult, PaymentRequest>(Arg.Any<string>(), Arg.Any<PaymentRequest>());
        }
        [Fact]
        public async Task TransactionReversal_ShouldReturnNyaradzoResultModel_Test()
        {
            //Arrange
            var reversalResult = DbContextFaker.GenerateNyaradzoReversalResultTestData();
            var service = Substitute.For<IAPIService>();
            service.Get<ReversalResult>(Arg.Any<string>()).Returns(reversalResult);
            var sut = new NyaradzoService(service);
            //Act
            var result =await sut.TransactionReversalAsync("Reference");
            //Assert
            result.Should().NotBeNull();
            result.ValidResponse.Should().BeTrue();
            await service.Received().Get<ReversalResult>(Arg.Any<string>());
        }
        [Fact]
        public async Task AccountEnquiry_ShouldReturnNyaradzoResultModel_Test()
        {
            //Arrange
            AccountSummary accountSummary = DbContextFaker.GenerateNyaradzoAccountSummaryTestData(); 
            var service = Substitute.For<IAPIService>();
            service.Get<AccountSummary>(Arg.Any<string>()).Returns(accountSummary);
            var sut = new NyaradzoService(service); 
            //Act
            var result =await sut.AccountEnquiryAsync("PolicyNumber", "Reference");
            //Assert
            result.Should().NotBeNull();
            result.ValidResponse.Should().BeTrue();
            await service.Received().Get<AccountSummary>(Arg.Any<string>());
        }
    }
}
