using FluentAssertions;
using System.Threading;
using System.Threading.Tasks; 
using TelOne.Application.AdslAccount;
using TelOne.Application.Common.Interfaces;
using TelOne.Application.Common.Models;
using TelOne.Infrastructure.Services;
using Xunit;
using static TelOne.Application.AdslAccount.RechargeAdslAccountCommand;

namespace Telone.Tests.ApplicationTests
{
    public class RechargeAdslAccountTests
    {
     
        [Fact]
        public async Task CanHandleSuccessAsync()
        {
            string Content = @"{
    ""ResponseCode"": ""118100138"",
    ""ResponseDescription"": ""UVC system error."",
    ""AccountNumber"": ""0242576644"",
    ""MerchantReference"": ""Test-004"",
    ""OrderNumber"": ""TBBP0000003642"",
    ""Voucher"": [
        {
                ""ProductId"": 91,
            ""PIN"": ""2979555881691039"",
            ""BatchNumber"": ""5000250245"",
            ""CardNumber"": ""5000250245060991""
        }
    ]
}";

            var httpfactory = Common.GetFakeFactory(Content);
            IAPIService service = new APIService(httpfactory);
             
            var item = new RechargeAdslAccountRequest() { };
            var request = new RechargeAdslAccountCommand(item);

            var cmd = new RechargeAdslAccountCommandHandler(service);
            var response = await cmd.Handle(request, new CancellationToken());

            response.Should().NotBeNull();
            response.IsT0.Should().Be(true);

            var result = response.AsT0;
            result.ResponseCode.Should().Be("118100138");
            result.ResponseDescription.Should().Be("UVC system error.");
            result.OrderNumber.Should().Be("TBBP0000003642");
            result.AccountNumber.Should().Be("0242576644");
            result.Voucher.Should().NotBeNull();
            result.Voucher[0].ProductId.Should().Be(91);
            result.Voucher[0].PIN.Should().Be("2979555881691039");
            result.Voucher[0].CardNumber.Should().Be("5000250245060991");
            result.Voucher[0].BatchNumber.Should().Be("5000250245"); 

        }


    }
}
