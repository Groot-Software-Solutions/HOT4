using Bogus;
using FluentAssertions;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models.RechargeServiceModels.Nyaradzo;
using Hot.Nyaradzo.Application.Actions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Hot.Nyaradzo.Test.Actions
{
    public class QueryNyaradzoAccountTests
    {
        [Fact]
        public async Task QueryNyaradzoAccount_ShouldReturnNyaradzoResult_Test()
        {
            //Arrange
            var nyaradzoResult = GenerateNyaradzoResult();
            var logger = Substitute.For<ILogger<QueryNyaradzoAccountHandler>>();
            var service = Substitute.For<INyaradzoRechargeAPIService>();
            service.QueryAccount(Arg.Any<string>()).Returns(nyaradzoResult);
            var request = new QueryNyaradzoAccount("SP098490");
            var sut = new QueryNyaradzoAccountHandler(service, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.Successful.Should().Be(true);
            await service.Received().QueryAccount(Arg.Is<string>("SP098490"));

        }

        private NyaradzoResult GenerateNyaradzoResult()
        {
            var nyaradzoResultFaker = new Faker<NyaradzoResult>()
                .RuleFor(a => a.Successful, true)
                .RuleFor(a => a.RawResponseData, f => f.Random.String())
                .RuleFor(a => a.TransactionResult, f => f.Random.String());
            return nyaradzoResultFaker.Generate();
        }
    
    }
}
