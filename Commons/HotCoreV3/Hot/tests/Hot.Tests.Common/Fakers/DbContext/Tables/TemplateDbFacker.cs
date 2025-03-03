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

public static class TemplateDbFacker
{
    public static IDbContext RandomList(this DbFakerService service, int count = 5)
    {
        service.dbContext.Templates.Search(Arg.Any<string>()).Returns(GetList(count));
        service.dbContext.Templates.SearchAsync(Arg.Any<string>()).Returns(GetList(count));
        return service.dbContext;
    }

    public static IDbContext RandomTemplate(this DbFakerService service)
    {
        return Template(service, GetSingle());
    }

    public static IDbContext Template(this DbFakerService service, Template template)
    {
        service.dbContext.Templates.Get(Arg.Any<int>()).Returns(template);
        service.dbContext.Templates.GetAsync(Arg.Any<int>()).Returns(template);
        return service.dbContext;
    }
    public static IDbContext GetTemplate(this DbFakerService service, Template template)
    {
        service.dbContext.Templates.Get(Arg.Any<int>()).Returns(template);
        return service.dbContext;
    }

    public static Template GetSingle()
    {
        return GetFaker().Generate();
    }

    public static List<Template> GetList(int count)
    {
        return GetFaker().Generate(count);
    }
    private static Faker<Template> GetFaker()
    {
        var templateFaker = new Faker<Template>()
        .RuleFor(a => a.TemplateId, f => f.Random.Int())
        .RuleFor(a => a.TemplateId, f => f.Random.Int())
        .RuleFor(a => a.TemplateText, "Success %MOBILE% Recharged with $%AMOUNT% Commission % DISCOUNT %% Cost $% COST %  Balance: $% BALANCE % Sale Value $% SALEVALUE % Thank you for choosing HOT Recharge");
        return templateFaker;
    }
    
}

