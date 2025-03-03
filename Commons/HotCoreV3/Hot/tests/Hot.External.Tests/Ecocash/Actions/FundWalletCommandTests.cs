using Bogus;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hot.Ecocash.Application.Actions.FundWalletCommand;

namespace Hot.Ecocash.Tests.Actions
{

    public class FundWalletCommandTests
    {
        private readonly IDbContext _dbcontext =
           Substitute.For<IDbContext>();
        //public async Task CreateBankTrxTest_ShouldReturnBankTrx()
        //{  
        //    //Arrange
        //    //var logger = Substitute.For<ILogger<FundWalletCommandHandler>>();
        //    //var ecocash = Substitute.For <IEcocashServiceFactory<FundWalletCommandHandler>>();

        //    //var access = GenerateAccessTestData();
        //    //_dbcontext.Accesss.SelectCodeAsync("0773404368").Returns(access);

        //    //var request = new FundWalletCommand();
        //    //var sut = new FundWalletCommandHandler(_dbcontext, logger);
        //    //Act

        //    //Assert
        //}

    
        public static Access GenerateAccessTestData()
        {
            DateTime startDate = Convert.ToDateTime("06/16/2021 00:00:00");
            var accessfaker = new Faker<Access>()
                .RuleFor(a => a.AccessId, f => f.Random.Long())
                .RuleFor(a => a.AccountId, f => f.Random.Long())
                .RuleFor(a => a.AccessCode, f => f.Random.String())
                .RuleFor(a => a.ChannelID, f => f.Random.Long())
                .RuleFor(a => a.AccessPassword, f => f.Random.String())
                .RuleFor(a => a.Deleted, f => f.Random.Bool())
                .RuleFor(a => a.PasswordHash, f => f.Random.String())
                .RuleFor(a => a.PasswordSalt, f => f.Random.String())
                .RuleFor(a => a.InsertDate, f => f.Date.Between(startDate, DateTime.Now));

            return accessfaker.Generate();
        }
    }
}
