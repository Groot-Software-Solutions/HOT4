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

public static class NetworkActionsDbFacker
{
    public static IDbContext RandomNetworksList(this DbFakerService service, int count = 5)
    {
        service.dbContext.Networks.List().Returns(GetList(count));
        service.dbContext.Networks.ListAsync().Returns(GetList(count));
        return service.dbContext;
    }
    public static Network GetSingle()
    {
        return GetFaker().Generate();
    }

    public static List<Network> GetList(int count)
    {
        return GetFaker().Generate(count);
    }
    private static Faker<Network> GetFaker()
    {
        var banksfaker = new Faker<Network>()
             .RuleFor(a => a.NetworkId, f => f.Random.Byte())
             .RuleFor(a => a.NetworkName, f => f.Name.ToString())
             .RuleFor(a => a.Prefix, f => f.Name.ToString())
             .RuleFor(a => a.EndPointAddress, f => f.Name.ToString())
             .RuleFor(a => a.EndPointAddressTest, f => f.Name.ToString());


        return banksfaker;
    }

}
