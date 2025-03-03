using AutoMapper;
using Hot.Application.Actions.NetworkActions;
using Hot.Tests.Common.Fakers.DbContext;
using Hot.Tests.Common.Fakers.DbContext.Tables;
using Hot.Tests.Common.Fakers.Services;

namespace Hot.Application.Tests.Actions
{
    public class NetworkBalancesActionsTests
    {
        private readonly IMapper mapper =
       Substitute.For<IMapper>();

        [Fact]
        public async Task GetNetworkBalanceById_ShouldReturnNetworkBalance_Test()
        {
            //Arrange
            var balance = NetworkBalanceDbFacker.GetSingle();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().NetworkBalance(balance);
            var logger = LoggerFaker.GetLogger<GetNetworkBalanceByIdQueryHandler>();
            var request = new GetNetworkBalanceByIdQuery(3);
            var sut = new GetNetworkBalanceByIdQueryHandler(_dbcontext,logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.Should().Be(balance);
            await _dbcontext.Received().NetworkBalance.GetByIdAsync(Arg.Is<int>(a => a == request.BrandId));
        }
        [Fact]
        public async Task GetNewBalance_ShouldReturnList_Test()
        {
            //Arrange
            var balances = NetworkBalanceDbFacker.GetNetworkBalanceList(10);
            var logger = LoggerFaker.GetLogger<GetNewBalancesQueryHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomNetworkBalanceList(10);
            var request = new GetNewBalancesQuery();
            var sut = new GetNewBalancesQueryHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Count.Should().Be(10);
            await _dbcontext.Received().NetworkBalance.ListAsync();
        }
      
    }
}
