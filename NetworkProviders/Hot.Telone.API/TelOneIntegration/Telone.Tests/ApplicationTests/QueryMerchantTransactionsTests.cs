using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TelOne.Application.Commands.Accounts;
using TelOne.Application.Commands.Merchant;
using TelOne.Application.Common.Interfaces;
using TelOne.Application.Common.Models;
using TelOne.Infrastructure.Services;
using Xunit;
using static TelOne.Application.Commands.Accounts.PayBillCommand;

namespace Telone.Tests.ApplicationTests
{
    public class QueryMerchantTransactionsTests
    {
        [Fact]
        public async Task CanHandleSuccessAsync()
        {
            string Content = @"[
  {
    ""OrderNumber"": ""12345678"",
    ""MerchantReference"": ""Hot-1234562"",
    ""Amount"": 1.0,
    ""ResultCode"": ""00"",
    ""Status"": ""Success"",
    ""TransactionDatetime"": ""2021-03-16T15:28:39.7391174+02:00""
  },
  {
    ""OrderNumber"": ""98765423"",
    ""MerchantReference"": ""Hot-8765432"",
    ""Amount"": 10.0,
    ""ResultCode"": ""00"",
    ""Status"": ""Success"",
    ""TransactionDatetime"": ""2021-03-16T15:38:39.7391174+02:00""
  }
]
";
            var httpfactory = Common.GetFakeFactory(Content);
            IAPIService service = new APIService(httpfactory);

            QueryMerchantTransactionsRequest item = new QueryMerchantTransactionsRequest() { };
            var request = new QueryMerchantTransactionsCommand(item);

            var cmd = new QueryMerchantTransactionsCommandHandler(service);
            var response = await cmd.Handle(request, new CancellationToken());

            response.Should().NotBeNull();
            response.IsT0.Should().Be(true);

            var result = response.AsT0;
            result.Should().NotBeEmpty();
            result.Should().HaveCountGreaterThan(1);
            result[0].OrderNumber.Should().Be("12345678");
            result[0].MerchantReference.Should().Be("Hot-1234562");
            result[0].Amount.Should().Be(1);
            result[0].ResultCode.Should().Be("00");
            result[0].Status.Should().Be("Success");
            result[0].TransactionDatetime.Should().Be(DateTime.Parse("2021-03-16T15:28:39.7391174+02:00"));
        }
    }
}
