using Bogus;
using FluentAssertions;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities; 
using Microsoft.Extensions.Logging; 

namespace Hot.Ecocash.Tests.Actions
{
    
    public class ComplateSelfTopUpTests
    {
        //private readonly IDbContext _dbcontext =
        //  Substitute.For<IDbContext>();

        //[Fact]
        //public async Task CompleteSelfTopUpCommand_TestAsync()
        //{
        //    var logger = Substitute.For<ILogger<CompleteSelfTopUpCommandHandler>>();
        //    //Arrange 
        //    var bankTrx = GenerateTestBankTrx();
        //    var request = new CompleteSelfTopUpCommand(bankTrx);
        //    var sut = new CompleteSelfTopUpCommandHandler(_dbcontext, logger);
        //    //Act
        //    var result = await sut.Handle(request, new CancellationToken());
        //    //Assert;
        //    result.Should().NotBeNull();
        //    var rec = result.AsT0;

        //}

        //private BankTrx GenerateTestBankTrx()
        //{
        //    DateTime startDate = Convert.ToDateTime("06/06/2021 00:00:00");
        //    var banksfaker = new Faker<BankTrx>()
        //        .RuleFor(a => a.BankTrxID, f => f.Random.Long())
        //        .RuleFor(a => a.BankTrxBatchID, f => f.Random.Long())
        //        .RuleFor(a => a.BankTrxTypeID, f => f.Random.Byte())
        //        .RuleFor(a => a.BankTrxStateID, f => f.Random.Byte())
        //        .RuleFor(a => a.Amount, f => f.Random.Decimal())
        //        .RuleFor(a => a.Balance, f => f.Random.Decimal())
        //        .RuleFor(a => a.TrxDate, f => f.Date.Between(startDate, DateTime.Now))
        //        .RuleFor(a => a.Identifier, f => f.Name.ToString())
        //        .RuleFor(a => a.RefName, f => f.Name.ToString())
        //        .RuleFor(a => a.Branch, f => f.Name.ToString())
        //        .RuleFor(a => a.PaymentID, f => f.Random.Long());
        //        return banksfaker.Generate();
          
        //}
    }
}
