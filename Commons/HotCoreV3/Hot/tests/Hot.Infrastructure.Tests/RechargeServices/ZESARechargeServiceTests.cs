using Bogus;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Models.RechargeServiceModels.ZESA;
using Hot.Domain.Entities;
using Hot.Tests.Common.Assertions;
using Hot.Tests.Common.Fakers;
using Hot.Infrastructure.RechargeServices.RechargeHandlers.Utilities;

namespace Hot.Infrastructure.Tests.RechargeServices;
public class ZesaRechargeServiceTests
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
    public async Task Recharge_Test(ZESAPurchaseTokenResult serviceResult, bool expected, byte resultState)
    {
        //Arrange    
        var service = RechargeAPIService(serviceResult);
        TestEnvironment.GetRechargeTestItems(service, out IServiceProvider serviceProvider, out IDbContext dbcontext, out Recharge recharge, out RechargePrepaid rechargePrepaid);
        var sut = new ZesaRechargeHandler(dbcontext, serviceProvider);
        //Act
        var result = await sut.ProcessAsync(recharge, rechargePrepaid);

        //Assert 
        await RechargeAssertions.AssertRechargeResult(expected, resultState, dbcontext, recharge, result, serviceResult.RawResponseData ?? "");
        await service.Received().PurchaseZesaToken(Arg.Is<string>(m => m == recharge.Mobile), Arg.Is<decimal>(amt => amt == recharge.Amount), Arg.Any<string>(), Arg.Any<Domain.Enums.Currency>());

    }



    private IZESARechargeAPIService RechargeAPIService(ZESAPurchaseTokenResult rechargeResult)
    {
        var service = Substitute.For<IZESARechargeAPIService>();
        service.PurchaseZesaToken(Arg.Any<string>(), Arg.Any<decimal>(), Arg.Any<string>(), Arg.Any<Domain.Enums.Currency>()).Returns(rechargeResult);
        return service;
    }

    private static ZESAPurchaseTokenResult SuccessulfulResult()
    {
        var result = RandomResult();
        result.Successful = true;
        return result;
    }
    private static ZESAPurchaseTokenResult FailedResult()
    {
        var result = RandomResult();
        result.Successful = false;
        return result;
    }
    private static ZESAPurchaseTokenResult RandomResult()
    {
        var rechargeResultFaker = new Faker<ZESAPurchaseTokenResult>()
            .RuleFor(a => a.Successful, true)
            .RuleFor(a => a.RawResponseData, f => f.Random.String())
            .RuleFor(a => a.TransactionResult, f => f.Random.String());
        return rechargeResultFaker.Generate();
    }
}
