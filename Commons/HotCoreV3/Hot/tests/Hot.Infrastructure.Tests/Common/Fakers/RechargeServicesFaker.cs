using Bogus;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models.RechargeServiceModels.Econet;
using Hot.Domain.Entities;
using Hot.Tests.Common.Fakers.DbContext;
using Hot.Tests.Common.Fakers.DbContext.Tables;

namespace Hot.Infrustructure.Tests.Common.Fakers
{
    public static class RechargeServicesFaker
    {
    
        private static void ArrangeForRecharge(out IDbContext _dbcontext, out EconetRechargeResult econetRechargeResult, out IEconetRechargeAPIService service, out Recharge recharge, out RechargePrepaid rechargePrepaid)
        {
            _dbcontext = DbContextFaker.GetDbContext();
            econetRechargeResult = GenerateEconetRechargeResultTestData();
            service = Substitute.For<IEconetRechargeAPIService>();
            var access = AccessDbFacker.GetSingle();
            var account = AccountDbFacker.GetSingle(); ;
            _dbcontext.Accounts.GetAsync(Arg.Any<int>()).Returns(account);
            _dbcontext.Accesss.GetAsync(Arg.Any<int>()).Returns(access);
            recharge = RechargeDbFacker.GetSingle();
            rechargePrepaid = RechargePrepaidDbFacker.GetSingle();
            // _serviceProvider.GetService(typeof(IEconetRechargePrepaidAPIService)).Returns(service);
        }

        private static EconetRechargeResult GenerateEconetRechargeResultTestData()
        {
            var econetRechargeResult = new Faker<EconetRechargeResult>();

            return econetRechargeResult.Generate();
        }
    }
}
