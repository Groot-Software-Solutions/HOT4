using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using TelOne.Application.BroadbandProducts;
using TelOne.Application.Common.Interfaces;
using TelOne.Application.Common.Models;
using TelOne.Infrastructure.Services;
using Xunit;
using static TelOne.Application.BroadbandProducts.PurchaseBroadbandProductsCommand;
using static TelOne.Application.BroadbandProducts.QueryBroadbandProductsCommand;


namespace Telone.Tests.ApplicationTests
{
    public class PurchaseBroadbandProductTests
    {
        [Fact]
        public async Task CanHandleSuccessAsync()
        {
            string Content = @"{
""ResponseCode"": ""00"", ""ResponseDescription"": ""Successful"", ""MerchantReference"": ""REF1234"", ""OrderNumber"": ""BP100343563"",
""Vouchers"": [ 
                { ""ProductId"": 101,""PIN"": ""123456789000"", ""BatchNumber"": ""5000250102"", ""SerialNumber"": ""50002501520003"" }, 
                { ""ProductId"": 102, ""PIN"": ""123456789000"", ""BatchNumber"": ""5000250102"", ""SerialNumber"": ""50002501520003"" } 
            ]}";

            var httpfactory = Common.GetFakeFactory(Content);
            IAPIService service = new APIService(httpfactory);

            var item = new PurchaseBroadbandProductsRequest() { };
            var request = new PurchaseBroadbandProductsCommand(item);

            var cmd = new PurchaseBroadbandProductsCommandHandler(service);
            var response = await cmd.Handle(request, new CancellationToken());

            response.Should().NotBeNull();
            response.IsT0.Should().Be(true);

            var result = response.AsT0;
            result.ResponseCode.Should().Be("00");
            result.ResponseDescription.Should().Be("Successful");
            result.OrderNumber.Should().Be("BP100343563");
            result.MerchantReference.Should().Be("REF1234");
            result.Vouchers.Should().NotBeNull();
            result.Vouchers.Count.Should().Be(2);
            result.Vouchers[0].ProductId.Should().Be(101);
            result.Vouchers[0].PIN.Should().Be("123456789000");
            result.Vouchers[0].SerialNumber.Should().Be("50002501520003");
            result.Vouchers[0].BatchNumber.Should().Be("5000250102");
            result.Vouchers[1].ProductId.Should().Be(102);
            result.Vouchers[1].PIN.Should().Be("123456789000");
            result.Vouchers[1].SerialNumber.Should().Be("50002501520003");
            result.Vouchers[1].BatchNumber.Should().Be("5000250102");



        }
    }
}
