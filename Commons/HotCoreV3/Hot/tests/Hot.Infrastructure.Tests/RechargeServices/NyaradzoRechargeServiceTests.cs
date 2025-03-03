using Bogus;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models.RechargeServiceModels.Nyaradzo;
using Hot.Domain.Entities;
using Hot.Tests.Common.Fakers;
using Hot.Tests.Common.Assertions;
using Hot.Infrastructure.RechargeServices.RechargeHandlers.Utilities;

namespace Hot.Infrastructure.Tests.RechargeServices;
public class NyaradzoRechargeServiceTests
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
    public async Task Recharge_Test(NyaradzoResult serviceResult, bool expected, byte resultState)
    {
        //Arrange    
        var service = RechargeAPIService(serviceResult);
        TestEnvironment.GetRechargeTestItems(service, out IServiceProvider serviceProvider, out IDbContext dbcontext, out Recharge recharge, out RechargePrepaid rechargePrepaid);
        var sut = new NyaradzoRechargeHandler(dbcontext, serviceProvider);
        //Act
        var result = await sut.ProcessAsync(recharge, rechargePrepaid);
        //Assert
        await RechargeAssertions.AssertRechargeResult(expected, resultState, dbcontext, recharge, result, serviceResult.RawResponseData ?? "");
        await service.Received().ProcessPayment(Arg.Is<NyaradzoPaymentRequest>(m => m.PolicyNumber == recharge.Mobile), Domain.Enums.Currency.ZiG);
    }

    private INyaradzoRechargeAPIService RechargeAPIService(NyaradzoResult rechargeResult)
    {
        var service = Substitute.For<INyaradzoRechargeAPIService>();
        service.ProcessPayment(Arg.Any<NyaradzoPaymentRequest>(), Arg.Any<Domain.Enums.Currency>()).Returns(rechargeResult);
        return service;
    }

    private static NyaradzoResult SuccessulfulResult()
    {
        var result = RandomResult();
        result.Successful = true;
        return result;
    }
    private static NyaradzoResult FailedResult()
    {
        var result = RandomResult();
        result.Successful = false;
        return result;
    }
    private static NyaradzoResult RandomResult()
    {
        var resultFaker = new Faker<NyaradzoResult>()
             .RuleFor(a => a.Successful, f => f.Random.Bool())
             .RuleFor(a => a.RawResponseData, f => f.Random.String())
             .RuleFor(a => a.Account, new NyaradzoAccountSummary())
             .RuleFor(a => a.TransactionResult, f => f.Random.String());
        return resultFaker.Generate();
    }

}
