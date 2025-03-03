using Hot.Tests.Common.Fakers.Services;
using Hot.Tests.Common.Fakers.DbContext.Tables;
using Hot.Tests.Common.Fakers.DbContext;
using Hot.Application.Actions.NetworkActions;

namespace Hot.Application.Tests.Actions
{
    public class NetworkActionTests
    {

        [Fact]
        public async Task SearchNetworks_ShouldReturnList_Test()
        {
            //Arrange
            var logger = LoggerFaker.GetLogger<GetNetworksQueryHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomNetworksList(10);
            var request = new GetNetworksQuery();
            var sut = new GetNetworksQueryHandler(_dbcontext, logger);



            //Act
            var result = await sut.Handle(request, new CancellationToken());



            //Assert
            result.IsT0.Should().Be(true);
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Should().HaveCount(10);
            await _dbcontext.Received().Networks.ListAsync();

        }

    }
}
