using Hot.Ecocash.Application.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Hot.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Hot.Ecocash.Domain.Entities;
using System.Threading;
using Hot.Domain.Entities;
using FluentAssertions;

namespace Hot.Ecocash.Tests.Actions
{
    public class SendEcocashConfirmationToAPIClientTests
    {

        [Fact]
        public async Task SendEcocashConfirmationToAPIClient_SuccesfullyAsync()
        {
            //Arrange 
            var apiService = Substitute.For<IAPIService>();
            apiService.Post<object, Transaction>(Arg.Any<string>(), Arg.Any<Transaction>()).Returns("Success");
            var logger = Substitute.For<ILogger<SendEcocashConfirmationToAPIClientHandler>>();  
            var request = new SendEcocashConfirmationToAPIClient(new Transaction(), new Account() { Email="http://test.com"});

            var sut = new SendEcocashConfirmationToAPIClientHandler(apiService, logger);

            //Act 
            var result  =  await sut.Handle(request, new CancellationToken());

            //Assert
            result.Should().Be(true);
 
        }

    }
}
