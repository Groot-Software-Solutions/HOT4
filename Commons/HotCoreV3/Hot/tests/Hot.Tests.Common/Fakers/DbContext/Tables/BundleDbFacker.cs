using Bogus;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Tests.Common.Fakers.DbContext.Tables;

public static class BundlesDbFacker
{
    public static IDbContext RandomBundlesList(this DbFakerService service, int count = 5)
    {     
        service.dbContext.Bundles.List().Returns(GetList(count));
        service.dbContext.Bundles.ListAsync().Returns(GetList(count));
        return service.dbContext;
    }
    public static Bundle  GetSingle()
    {
        return GetFaker().Generate();
    }

    public static List<Bundle> GetList(int count)
    {
        return GetFaker().Generate(count);
    }
    private static Faker<Bundle> GetFaker()
    {
        var bundlesfaker = new Faker<Bundle>()
             .RuleFor(a => a.BundleID, f => f.Random.Int())
             .RuleFor(a => a.BrandID, f => f.Random.Int())
             .RuleFor(a => a.Name, f => f.Commerce.Product())
             .RuleFor(a => a.Description, f => f.Name.ToString())
             .RuleFor(a => a.Amount, f => f.Random.Int())
             .RuleFor(a => a.ProductCode, f => f.Name.ToString())
             .RuleFor(a => a.ValidityPeriod, f => f.Random.Int())
             .RuleFor(a => a.Amount, f => f.Random.Int())
             .RuleFor(a => a.Enabled, f => f.Random.Bool())
             .RuleFor(a => a.Network, f => f.Name.ToString());          

        return bundlesfaker;
    }

}
