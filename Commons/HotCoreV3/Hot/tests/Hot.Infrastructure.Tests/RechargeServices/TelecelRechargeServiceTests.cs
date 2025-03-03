using Bogus;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models.RechargeServiceModels.Telecel;

using Hot.Domain.Entities;
using Hot.Tests.Common.Fakers;
using Hot.Tests.Common.Assertions;
using Hot.Infrastructure.RechargeServices.RechargeHandlers.Airtime;

namespace Hot.Infrastructure.Tests.RechargeServices;
public class TelecelRechargeServiceTests
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
    public async Task Recharge_Test(TelecelRechargeResult serviceResult, bool expected, byte resultState)
    {
        //Arrange    
        var service = RechargeAPIService(serviceResult);
        TestEnvironment.GetRechargeTestItems(service, out IServiceProvider serviceProvider, out IDbContext dbcontext, out Recharge recharge, out RechargePrepaid rechargePrepaid);
        var sut = new TelecelRechargeHandler(dbcontext, serviceProvider);
        //Act
        var result = await sut.ProcessAsync(recharge, rechargePrepaid);

        //Assert 
        await RechargeAssertions.AssertRechargeResult(expected, resultState, dbcontext, recharge, result, serviceResult.RawResponseData ?? "");
        await service.Received().Recharge(Arg.Is<string>(m => m == recharge.Mobile), Arg.Is<decimal>(amt => amt == recharge.Amount), Arg.Any<string>(), Arg.Any<Domain.Enums.Currency>());

    }



    private ITelecelRechargeAPIService RechargeAPIService(TelecelRechargeResult rechargeResult)
    {
        var service = Substitute.For<ITelecelRechargeAPIService>();
        service.Recharge(Arg.Any<string>(), Arg.Any<decimal>(), Arg.Any<string>(), Arg.Any<Domain.Enums.Currency>()).Returns(rechargeResult);
        return service;
    }

    private static TelecelRechargeResult SuccessulfulResult()
    {
        var result = RandomResult();
        result.Successful = true;
        return result;
    }
    private static TelecelRechargeResult FailedResult()
    {
        var result = RandomResult();
        result.Successful = false;
        return result;
    }
    private static TelecelRechargeResult RandomResult()
    {
        var rechargeResultFaker = new Faker<TelecelRechargeResult>()
            .RuleFor(a => a.Successful, true)
            .RuleFor(a => a.RawResponseData, f => f.Random.String())
            .RuleFor(a => a.TransactionResult, f => f.Random.String());
        return rechargeResultFaker.Generate();
    }
}

