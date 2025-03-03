using FluentAssertions; 
using System.Threading;
using System.Threading.Tasks;
using TelOne.Application.Accounts;
using TelOne.Application.Common.Interfaces;
using TelOne.Application.Common.Models;
using TelOne.Infrastructure.Services;
using Xunit;
using static TelOne.Application.Accounts.VerifyCustomerAccountCommand;

namespace Telone.Tests.ApplicationTests
{

    public class VerifyCustomerAccountTests
    {
        [Fact]
        public async Task CanHandleSuccessAsync()
        {
            string Content = @"{
""AccountNumber"": ""0242576644"", ""AccountName"": ""John Doe"",
""AccountAddress"": ""123 Main Street, Harare"", ""ResponseCode"": ""00"",
""ResponseDescription"": ""Successful""
}";

            var httpfactory = Common.GetFakeFactory(Content);
            IAPIService service = new APIService(httpfactory);


            CustomerAccountRequest item = new CustomerAccountRequest() { };
            var request = new VerifyCustomerAccountCommand(item);

            var cmd = new VerifyCustomerAccountCommandHandler(service);
            var response = await cmd.Handle(request, new CancellationToken());

            response.Should().NotBeNull();
            response.IsT0.Should().Be(true);

            var result = response.AsT0;
            result.ResponseCode.Should().Be("00");
            result.ResponseDescription.Should().Be("Successful");
            result.AccountAddress.Should().Be("123 Main Street, Harare");
            result.AccountName.Should().Be("John Doe");
            result.AccountNumber.Should().Be("0242576644");

        }


    }
}
