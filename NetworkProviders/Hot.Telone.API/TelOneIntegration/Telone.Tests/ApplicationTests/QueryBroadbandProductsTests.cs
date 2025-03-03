using FluentAssertions;
using System.Threading;
using System.Threading.Tasks; 
using TelOne.Application.BroadbandProducts;
using TelOne.Application.Common.Interfaces;
using TelOne.Application.Common.Models;
using TelOne.Infrastructure.Services;
using Xunit; 
using static TelOne.Application.BroadbandProducts.QueryBroadbandProductsCommand;

namespace Telone.Tests.ApplicationTests
{
   public  class QueryBroadbandProductsTests
    {

        [Fact]
        public async Task CanHandleSuccessAsync()
        {
            string Content = @"[
{
""ProductId"": 101,
""ProductName"": ""Infinity Supreme"",
""ProductDescription"": ""Upto 3MBps, Download Cap Unlimited"", ""Activated"": true,
""ProductPrices"": [{""Currency"": ""RTGS"", ""Price"": 220.00}, {""Currency"": ""USD"",""Price"": 37.00 }] 
}, 
{
""ProductId"": 102,
""ProductName"": ""Infinity Pro"",
""ProductDescription"": "" Upto 5MBps, Download Cap Unlimited "", ""Activated"": true,
""ProductPrices"": [{""Currency"": ""RTGS"", ""Price"": 300.00}, {""Currency"": ""USD"",""Price"": 50.00 }] 
}
]";

            var httpfactory = Common.GetFakeFactory(Content);
            IAPIService service = new APIService(httpfactory);

            var item = new QueryBroadbandProductsRequest() { };
            var request = new QueryBroadbandProductsCommand(item);

            var cmd = new QueryBroadbandProductsCommandHandler(service);
            var response = await cmd.Handle(request, new CancellationToken());

            response.Should().NotBeNull();
            response.IsT0.Should().Be(true);

            var result = response.AsT0;

            result.Count.Should().Be(2);
            result[0].ProductId.Should().Be(101);
            result[0].ProductName.Should().Be("Infinity Supreme");
            result[0].ProductDescription.Should().Be("Upto 3MBps, Download Cap Unlimited");
            result[0].Activated.Should().Be(true);
            result[0].ProductPrices.Should().NotBeNull();
            result[0].ProductPrices.Count.Should().Be(2);
            result[0].ProductPrices[0].Currency.Should().Be("RTGS");
            result[0].ProductPrices[0].Price.Should().Be(220);
            result[0].ProductPrices[1].Currency.Should().Be("USD");
            result[0].ProductPrices[1].Price.Should().Be(37); result[0].ProductId.Should().Be(101);

            result[1].ProductId.Should().Be(102);
            result[1].ProductName.Should().Be("Infinity Pro");
            result[1].ProductDescription.Should().Be(" Upto 5MBps, Download Cap Unlimited ");
            result[1].Activated.Should().Be(true);
            result[1].ProductPrices.Should().NotBeNull();
            result[1].ProductPrices.Count.Should().Be(2);
            result[1].ProductPrices[0].Currency.Should().Be("RTGS");
            result[1].ProductPrices[0].Price.Should().Be(300);
            result[1].ProductPrices[1].Currency.Should().Be("USD");
            result[1].ProductPrices[1].Price.Should().Be(50);
                   
        }

    }
}
