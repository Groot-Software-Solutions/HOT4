using Hot.Application.Actions.BankTrxBatchActions;
using Hot.Tests.Common.Fakers.DbContext;
using Hot.Tests.Common.Fakers.Services;
using Hot.Tests.Common.Fakers.DbContext.Tables;

namespace Hot.Application.Tests.Actions
{
    public class BankTrxBatchActionsTests
    {
        [Fact]
        public async Task GetBankTrxBatchShouldReturnBankTrxBatches_Test()
        {
            //Arrange
            var bankTrxBatches = BankTrxBatchDbFacker.GetList(10);
            var logger = LoggerFaker.GetLogger<GetBankTrxBatchesHandler>();
            var _dbcontext = DbContextFaker.Mock()
               .Gives().RandomBankTrxBatchList(10);
            var request = new GetBankTrxBatches(123);
            var sut = new GetBankTrxBatchesHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().Be(true);
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Should().HaveCount(10);
            await _dbcontext.Received().BankTrxBatches.ListAsync(Arg.Is<byte>(a => a == request.BankId));
        }        
    }
}
