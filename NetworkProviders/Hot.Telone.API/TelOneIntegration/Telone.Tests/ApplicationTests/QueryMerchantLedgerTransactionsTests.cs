using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using TelOne.Application.Commands.Merchant;
using TelOne.Application.Common.Interfaces;
using TelOne.Application.Common.Models;
using TelOne.Infrastructure.Services;
using Xunit;
using FluentAssertions;

namespace Telone.Tests.ApplicationTests
{
   public class QueryMerchantLedgerTransactionsTests
    {
        [Fact]
        public async Task CanHandleSuccessAsync()
        {
            string Content = @"
[
  {
    ""ID"": 1,
    ""RefNumber"": ""hot1"",
    ""Amount"": 3.0,
    ""Balance"": 4.0,
    ""VoucherNum"": ""sample string 5"",
    ""TransactionDateTime"": ""2021-03-23T09:56:29.5117799+02:00"",
    ""Narrative"": ""sample string 7"",
    ""Cr"": true,
    ""TransType"": 1,
    ""Service"": 1,
    ""Currency"": ""sample string 9""
  },
  {
    ""ID"": 1,
    ""RefNumber"": ""sample string 2"",
    ""Amount"": 3.0,
    ""Balance"": 4.0,
    ""VoucherNum"": ""sample string 5"",
    ""TransactionDateTime"": ""2021-03-23T09:56:29.5117799+02:00"",
    ""Narrative"": ""sample string 7"",
    ""Cr"": true,
    ""TransType"": 1,
    ""Service"": 1,
    ""Currency"": ""sample string 9""
  }
]
";
            var httpfactory = Common.GetFakeFactory(Content);
            IAPIService service = new APIService(httpfactory);

            QueryMerchantTransactionsRequest item = new QueryMerchantTransactionsRequest() { };
            var request = new QueryMerchantTransactionsLedgerCommand(item);

            var cmd = new QueryMerchantTransactionsLedgerCommandHandler(service);
            var response = await cmd.Handle(request, new CancellationToken());

            response.Should().NotBeNull();
            response.IsT0.Should().Be(true);

            var result = response.AsT0;
            result.Should().NotBeEmpty();
            result.Should().HaveCountGreaterThan(1);
            result[0].ID.Should().Be(1);

        }
    }
}
