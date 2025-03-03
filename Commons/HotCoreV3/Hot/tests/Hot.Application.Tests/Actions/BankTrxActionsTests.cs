
using Hot.Application.Actions.BankTrxActions;
using Hot.Tests.Common.Fakers.DbContext;
using Hot.Domain.Entities;

using Hot.Tests.Common.Fakers.Services;
using Hot.Tests.Common.Fakers.DbContext.Tables;

namespace Hot.Application.Tests.Actions
{
    public class BankTrxActionsTests
    {
        [Fact]
        public async Task CreateBankTrx_ShouldReturnBankTrx_Test()
        {
            //Arrange
            var bankTrx = BankTrxDbFacker.GetSingle();
            int bankTrxId = (int)bankTrx.BankTrxID;
            var logger = LoggerFaker.GetLogger<CreateBatchTrxHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().BankTrxAdd();
            var request = new CreateBankTrxCommand(bankTrx);
            var sut = new CreateBatchTrxHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.Should().Be(bankTrx);
            await _dbcontext.Received().BankTrxs.AddAsync(Arg.Is<BankTrx>(a => a.Balance == bankTrx.Balance));
            await _dbcontext.Received().BankTrxs.AddAsync(Arg.Is<BankTrx>(a => a.Amount == bankTrx.Amount));
            await _dbcontext.Received().BankTrxs.AddAsync(Arg.Is<BankTrx>(a => a.PaymentID == bankTrx.PaymentID));
        }
        [Fact]
        public async Task SearchByBatchIdBankTrx_ShouldReturnList_Test()
        {
            //Arrange
            var bankTrx = BankTrxDbFacker.GetSingle();
            var logger = LoggerFaker.GetLogger<SearchBankTrxByBatchIdHandler>();
            var _dbcontext = DbContextFaker.Mock()
               .Gives().RandomBankTrxList(10);
            var request = new SearchBankTrxByBatchId(12345);
            var sut = new SearchBankTrxByBatchIdHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            //result.IsT0.Should().Be(true);
            var list = result.AsT0;
            if (list != null)
            {
                list.Should().NotBeEmpty();
                list.Should().HaveCount(10);
            }
            await _dbcontext.Received().BankTrxs.ListAsync(Arg.Is<long>(a => a == bankTrx.BankTrxBatchID));                
        }
        [Fact]
        public async Task SearchByReferenceBankTrx_ShouldReturnBatchTrx_Test()
        {
            //Arrange
            var bankTrx = BankTrxDbFacker.GetSingle();
            //var bankTrxList = DbContextFaker.GenerateBankTrxListTestdata(10);
            int bankTrxId = (int)bankTrx.BankTrxID;
            var logger = LoggerFaker.GetLogger<SearchBankTrxByReferenceHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomBankTrx();
            var request = new SearchBankTrxByReference(bankTrx.BankRef);
            var sut = new SearchBankTrxByReferenceHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            await _dbcontext.Received().BankTrxs.GetByRefAsync(Arg.Is<string>(a => a == bankTrx.BankRef));
        }
        [Fact]
        public async Task UpdateBankTrx_ShouldReturnsBoolean_Test() 
        {
            //Arrange
            var bankTrx = BankTrxDbFacker.GetSingle();
            var logger = LoggerFaker.GetLogger<UpdateBankTrxCommandHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().BankTrxUpdate(true);
            var request = new UpdateBankTrxCommand(bankTrx);
            var sut = new UpdateBankTrxCommandHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.Should().Be(true);
            await _dbcontext.Received().BankTrxs.UpdateAsync(Arg.Is<BankTrx>(a => a.BankTrxID == bankTrx.BankTrxID));
        }      
    }
}
