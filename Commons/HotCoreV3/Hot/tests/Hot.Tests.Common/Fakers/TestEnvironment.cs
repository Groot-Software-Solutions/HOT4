using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using Hot.Tests.Common.Fakers.DbContext;
using Hot.Tests.Common.Fakers.DbContext.Tables;
using Hot.Tests.Common.Fakers.Services;
using System;

namespace Hot.Tests.Common.Fakers
{
    public static class TestEnvironment
    {
        public static void GetRechargeTestItems<T>(T service, out IServiceProvider serviceProvider, out IDbContext dbcontext, out Recharge recharge, out RechargePrepaid rechargePrepaid)
        {
            serviceProvider = ServiceProviderFaker.Mock().Gives(service);
            recharge = RechargeDbFacker.GetSingle();
            rechargePrepaid = RechargePrepaidDbFacker.GetSingle();
            dbcontext = DbContextFaker.Mock()
                .Gives().RandomAccess()
                .Gives().RandomAccount()
                .Gives().RandomBrand();
        }
    }
}
