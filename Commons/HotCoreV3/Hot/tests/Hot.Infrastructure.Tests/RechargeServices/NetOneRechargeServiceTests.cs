using Bogus;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models.RechargeServiceModels.NetOne;
using Hot.Domain.Entities;
using Hot.Infrastructure.RechargeServices.RechargeHandlers.Airtime;
using Hot.Tests.Common.Assertions;
using Hot.Tests.Common.Fakers;

namespace Hot.Infrastructure.Tests.RechargeServices;
public class NetOneRechargeServiceTests
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
    public async Task Recharge_Test(NetOneRechargeResult serviceResult, bool expected, byte resultState)
    {
        //Arrange    
        var service = RechargeAPIService(serviceResult); 
        TestEnvironment.GetRechargeTestItems(service, out IServiceProvider serviceProvider, out IDbContext dbcontext, out Recharge recharge, out RechargePrepaid rechargePrepaid);
        var sut = new NetOneRechargeHandler(dbcontext, serviceProvider);
        //Act
        var result = await sut.ProcessAsync(recharge, rechargePrepaid);
        //Assert
        await RechargeAssertions.AssertRechargeResult(expected, resultState, dbcontext, recharge, result, serviceResult.RawResponseData ?? "");
        await service.Received().Recharge(Arg.Is<string>(m => m == recharge.Mobile), Arg.Is<decimal>(amt => amt == recharge.Amount), Arg.Any<int>(),Arg.Any<Domain.Enums.Currency>());
    }
     
    private INetOneRechargeAPIService RechargeAPIService(NetOneRechargeResult netOneRechargeResult)
    {
        var service = Substitute.For<INetOneRechargeAPIService>(); 
        service.Recharge(Arg.Any<string>(), Arg.Any<decimal>(), Arg.Any<int>(),Arg.Any<Domain.Enums.Currency>()).Returns(netOneRechargeResult);
        return service;
    }

    private static NetOneRechargeResult SuccessulfulResult()
    {
        var result = RandomResult();
        result.Successful = true;
        return result;
    }
    private static NetOneRechargeResult FailedResult()
    {
        var result = RandomResult();
        result.Successful = false;
        return result;
    }
    private static NetOneRechargeResult RandomResult()
    {
        var rechargeResultFaker = new Faker<NetOneRechargeResult>()
             .RuleFor(a => a.Successful, f => false)
             .RuleFor(a => a.RawResponseData, f => f.Random.String())
             .RuleFor(a => a.TransactionResult, f => f.Random.String());
        return rechargeResultFaker.Generate();
    }
}




