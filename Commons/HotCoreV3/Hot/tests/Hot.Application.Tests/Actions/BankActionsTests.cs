using Hot.Application.Actions.BankActions;
using Hot.Tests.Common.Fakers.Services;
using Hot.Tests.Common.Fakers.DbContext.Tables;
using Hot.Tests.Common.Fakers.DbContext;

namespace Hot.Application.Tests.Actions
{
    public class BankActionsTests
    { 
       
        [Fact]
        public async Task SearchBanks_ShouldReturnList_Test()
        {
            //Arrange
            var logger = LoggerFaker.GetLogger<GetBanksQueryHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomBanksList(10);
            var request = new GetBanksQuery();
            var sut = new GetBanksQueryHandler(_dbcontext, logger);
            
            //Act
            var result = await sut.Handle(request,new CancellationToken());

            //Assert
            result.IsT0.Should().Be(true);
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Should().HaveCount(10);
            await _dbcontext.Received().Banks.ListAsync();          
   
        }

    }
}
