using Bogus;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using Hot.Tests.Common.Assertions;
using Hot.Tests.Common.Fakers;
using Hot.Application.Common.Models.RechargeServiceModels.Telone;
using Hot.Infrastructure.RechargeServices.RechargeHandlers.Data;

namespace Hot.Infrastructure.Tests.RechargeServices;

public class TeloneRechargeServiceTests
{
    public static IEnumerable<object[]> RechargeTests()
    {
        return new List<object[]>
        {
            new object[]{ SuccessulfulResult(),true,2},
            new object[]{ FailedResult(),false,1}
        };
    }


    [Theory]
    [MemberData(nameof(RechargeTests))]
    public async Task Recharge_Test(TeloneRechargeResult serviceResult, bool expected, byte resultState)
    {
        //Arrange    
        var service = RechargeAPIService(serviceResult);
        TestEnvironment.GetRechargeTestItems(service, out IServiceProvider serviceProvider, out IDbContext dbcontext, out Recharge recharge, out RechargePrepaid rechargePrepaid);
        var sut = new TeloneRechargeHandler(dbcontext, serviceProvider);
        rechargePrepaid.Data = "1";
        //Act
        var result = await sut.ProcessAsync(recharge, rechargePrepaid);
        //Assert
        await RechargeAssertions.AssertRechargeResult(expected, resultState, dbcontext, recharge, result, serviceResult.RawResponseData ?? "");
        await service.Received().RechargeDataBundle(
            Arg.Any<string>(), 
            Arg.Any<int>(),
            Arg.Any<int>(), 
            Arg.Any<Domain.Enums.Currency>());
    }

    private ITeloneDataAPIService RechargeAPIService(TeloneRechargeResult rechargeResult)
    {
        var service = Substitute.For<ITeloneDataAPIService>();
        service.RechargeDataBundle(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<Domain.Enums.Currency>()).Returns(rechargeResult);
        return service;
    }

    private static TeloneRechargeResult SuccessulfulResult()
    {
        var result = RandomResult();
        result.Successful = true;
        return result;
    }
    private static TeloneRechargeResult FailedResult()
    {
        var result = RandomResult();
        result.Successful = false;
        return result;
    }
    private static TeloneRechargeResult RandomResult()
    {
        var resultFaker = new Faker<TeloneRechargeResult>()
             .RuleFor(a => a.Successful, f => f.Random.Bool())
             .RuleFor(a => a.RawResponseData, f => f.Random.String())
             .RuleFor(a => a.ResponseCode, f => f.Random.String2(2, "1234567890"))
             .RuleFor(a => a.TransactionResult, f => f.Lorem.Sentence(10))
             .RuleFor(a => a.FinalWallet, f => f.Random.Number(1000, 100000))
             .RuleFor(a => a.InitialWallet, f => f.Random.Number(100000, 1000000)) 
             .RuleFor(a => a.Vouchers, f=> new()) 
             .RuleFor(a => a.Reference, f => f.Random.String2(10));
        return resultFaker.Generate();
    }

}