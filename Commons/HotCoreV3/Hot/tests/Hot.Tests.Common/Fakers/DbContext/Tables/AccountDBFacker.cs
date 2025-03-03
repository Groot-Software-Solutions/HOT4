using Bogus;
using FluentAssertions;
//using Hot.API.Service.Models.Accounts;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Models;
using Hot.Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace Hot.Tests.Common.Fakers.DbContext.Tables;

public static class AccountDbFacker
{
    public static IDbContext RandomAccountList(this DbFakerService service, int count = 5)
    {
        service.dbContext.Accounts.Search(Arg.Any<string>()).Returns(GetList(count));
        service.dbContext.Accounts.SearchAsync(Arg.Any<string>()).Returns(GetList(count));
        return service.dbContext;
    }
    public static IDbContext Account(this DbFakerService service, Account account)
    {
        service.dbContext.Accounts.Get(Arg.Any<int>()).Returns(account);
        service.dbContext.Accounts.GetAsync(Arg.Any<int>()).Returns(account);
        return service.dbContext;
    }
    public static IDbContext AccountUpdate(this DbFakerService service, bool expectedResult)
    {
        service.dbContext.Accounts.Update(Arg.Any<Account>()).Returns(expectedResult);
        service.dbContext.Accounts.UpdateAsync(Arg.Any<Account>()).Returns(expectedResult);
        return service.dbContext;
    }
    public static IDbContext AccountAdd(this DbFakerService service, int expectedResult)
    {
        service.dbContext.Accounts.Add(Arg.Any<Account>()).Returns(expectedResult);
        service.dbContext.Accounts.AddAsync(Arg.Any<Account>()).Returns(expectedResult);
        return service.dbContext;
    }
    public static IDbContext RandomAccount(this DbFakerService service)
    {
        return Account(service, GetSingle());
    }
    public static Account GetSingle()
    {
        var accountsfaker = new Faker<Account>()
            .RuleFor(a => a.AccountID, f => f.Random.Long(1000000, 10000000))
            .RuleFor(a => a.Balance, f=>f.Random.Decimal(1000,1000000))
            .RuleFor(a => a.USDBalance, f=>f.Random.Decimal(1000, 1000000))
            .RuleFor(a => a.ZesaBalance, f=>f.Random.Decimal(1000, 1000000))
            .RuleFor(a => a.AccountName, f => f.Name.FullName())
            .RuleFor(a => a.ProfileID, f => f.Random.Int(1, 100))
            .RuleFor(a => a.NationalID, f => f.Random.AlphaNumeric(15))
            .RuleFor(a => a.Email, f => f.Internet.Email())
            .RuleFor(a => a.ReferredBy, f => f.Phone.PhoneNumber())
            .RuleFor(a => a.InsertDate, DateTime.Now);

        return accountsfaker.Generate();
    }
    //public static AccountRegistrationRequest GetAccountRegistrationRequestSingle()
    //{
    //    var ccountRegistrationRequestfaker = new Faker<AccountRegistrationRequest>()
    //        .RuleFor(a => a.AccountName, "Norlin")
    //        .RuleFor(a => a.Firstname, "Norlin")
    //        .RuleFor(a => a.Lastname, "Hundirwa")
    //        .RuleFor(a => a.Password, "1234")
    //        .RuleFor(a => a.IDNumber, "63-1430454-Z-45")
    //        .RuleFor(a => a.ReferredBy, "Nomsa");
    //    return ccountRegistrationRequestfaker.Generate();
    //}
    public static List<Account> GetList(int count)
    {
        var accountsfaker = new Faker<Account>()
            .RuleFor(a => a.AccountID, f => f.Random.Long())
            .RuleFor(a => a.AccountName, f => f.Name.ToString())
            .RuleFor(a => a.ProfileID, f => f.Random.Int())
            .RuleFor(a => a.NationalID, f => f.Name.ToString())
            .RuleFor(a => a.Email, f => f.Random.String())
            .RuleFor(a => a.ReferredBy, f => f.Name.ToString())
            .RuleFor(a => a.InsertDate, DateTime.Now);

        return accountsfaker.Generate(count);
    }
    public static AccountDetailedModel GetAccountModelListTestData(Account account)
    {
        var AccountDeailedModel = new Faker<AccountDetailedModel>()
             .RuleFor(a => a.AccountID, account.AccountID)
             .RuleFor(a => a.AccountName, account.AccountName);
        return AccountDeailedModel.Generate();
    }
}