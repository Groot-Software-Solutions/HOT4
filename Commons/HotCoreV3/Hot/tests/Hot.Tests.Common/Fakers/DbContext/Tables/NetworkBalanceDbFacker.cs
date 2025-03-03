using Bogus;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Models;
using Hot.Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Tests.Common.Fakers.DbContext.Tables;
public static class NetworkBalanceDbFacker
{
    public static IDbContext RandomNetworkBalanceList(this DbFakerService service, int count = 5)
    {
        service.dbContext.NetworkBalance.List().Returns(GetNetworkBalanceList(count));
        service.dbContext.NetworkBalance.ListAsync().Returns(GetNetworkBalanceList(count));
        return service.dbContext;
    }

    public static IDbContext RandomNetworkBalance(this DbFakerService service)
    {
        return NetworkBalance(service, GetSingle());
    }
    public static IDbContext Network(this DbFakerService service, int networkId)
    {
        service.dbContext.Networks.IndentifyAsync(Arg.Any<string>()).Returns(networkId);
        return service.dbContext;
    }

    public static IDbContext NetworkBalance(this DbFakerService service, NetworkBalance networkBalance)
    {
  
        service.dbContext.NetworkBalance.GetByIdAsync(Arg.Any<int>()).Returns(networkBalance);
        return service.dbContext;
    }
    public static NetworkBalance GetSingle()
    {
        return GetFaker().Generate();
    }
    public static List<NetworkBalance> GetNetworkBalanceList(int count)
    {
        return GetFaker().Generate(count);
    }
    private static Faker<NetworkBalance> GetFaker()
    {
        var networkBalanceFaker = new Faker<NetworkBalance>()
            .RuleFor(a => a.BrandId, f => f.Random.Int())
            .RuleFor(a => a.Name, f => f.Random.String())
            .RuleFor(a => a.Balance, f => f.Random.Decimal());
        return networkBalanceFaker;
    }
}

