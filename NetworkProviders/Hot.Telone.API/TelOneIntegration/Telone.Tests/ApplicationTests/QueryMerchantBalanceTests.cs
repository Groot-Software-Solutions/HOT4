using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TelOne.Application.Commands.Merchant;
using TelOne.Application.Common.Interfaces;
using TelOne.Application.Common.Models;
using TelOne.Infrastructure.Services;
using Xunit;
using static TelOne.Application.Commands.Merchant.QueryMerchantBalanceCommand;

namespace Telone.Tests.ApplicationTests
{
    public class QueryMerchantBalanceTests
    {
        [Fact]
        public async Task CanHandleSuccessAsync()
        {
            string Content = @"[
     {
    ""MerchantId"": 1,
       ""Service"": 2,
       ""Active"": true,
       ""Balance"": 4.0,
       ""CreditLimit"": 5.0,
       ""Currency"": ""sample string 6""
  },
  {
    ""MerchantId"": 1,
    ""Service"": 2,
    ""Active"": true,
    ""Balance"": 4.0,
    ""CreditLimit"": 5.0,
    ""Currency"": ""sample string 6""
  }
]";
            var httpfactory = Common.GetFakeFactory(Content);
            IAPIService service = new APIService(httpfactory);
             
            var request = new QueryMerchantBalanceCommand(new Request());

            var cmd = new QueryMerchantBalanceCommandHandler(service);
            var response = await cmd.Handle(request, new CancellationToken());

            response.Should().NotBeNull();
            response.IsT0.Should().Be(true);

            var result = response.AsT0;
            result.Count.Should().BeGreaterThan(0);
            result[0].Active.Should().Be(true);
        }

    }
}
