using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Configuration.Internal;
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
    public class PayBillTests
    { 
        [Fact]
        public async Task CanHandleSuccessAsync()
        {
            string Content = @"{ 
      ""return_description"": """",
    ""return_result"": ""0:1015:1274552880839"",
    ""return_message"": ""Payment created successfully"",
    ""return_code"": 0
}";
            var httpfactory = Common.GetFakeFactory(Content);
            IAPIService service = new APIService(httpfactory);

            PayBillRequest item = new PayBillRequest() { };
            var request = new PayBillCommand(item);

            var cmd = new PayBillCommandHandler(service);
            var response = await cmd.Handle(request, new CancellationToken());

            response.Should().NotBeNull();
            response.IsT0.Should().Be(true);

            var result = response.AsT0;
            result.return_result.Should().Be("0:1015:1274552880839");
            result.return_message.Should().Be("Payment created successfully");
            result.return_code.Should().Be(0);

        }
    }
}
