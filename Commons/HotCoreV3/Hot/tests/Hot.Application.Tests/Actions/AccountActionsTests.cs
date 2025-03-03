using AutoMapper;
using Hot.Application.Actions.AccountActions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Models;
using Hot.Tests.Common.Fakers.DbContext;
using Hot.Domain.Entities;
using Hot.Domain.Enums;
using System;
using System.Data;
using static Hot.Application.Actions.AccountActions.LinktoExisitngAccountCommand;
using Hot.Tests.Common.Fakers.Services;
using Hot.Tests.Common.Fakers.DbContext.Tables;
using Microsoft.Extensions.Configuration;
using Hot.Application.Common.Exceptions;

namespace Hot.Application.Tests.Actions
{
    public class AccountActionsTests
    {
        private readonly IDbHelper dbhelper =
         Substitute.For<IDbHelper>();
        private readonly IMapper mapper =
         Substitute.For<IMapper>();

        [Fact]
        public async Task AccountHasConfirmedDetails_ShouldReturnBoolen_Test()
        {
            var address = AddressDbFacker.GetSingle();
            long accountId = 12345;
            var request = new AccountHasConfirmedDetailsCommand(accountId);
            var _dbcontext = DbContextFaker.Mock()
                .Gives().GetAddress(address);
            var logger = LoggerFaker.GetLogger<AccountHasConfirmedDetailsCommandHandler>();
            var sut = new AccountHasConfirmedDetailsCommandHandler(_dbcontext, logger);

            //Act
            var result = await sut.Handle(request, new CancellationToken());

            //Assert
            result.IsT0.Should().BeTrue();
            await _dbcontext.Received().Addresses.GetAsync(Arg.Any<long>());
        }

        [Fact]
        public async Task AccountRegistration_ShouldReturnBoolen_Test()
        {
            //Arrange

            AccountType accountType = AccountType.Corporate;
            var access = AccessDbFacker.GetSingle();
            var config = Substitute.For<IConfiguration>();
            var dbconn = Substitute.For<IDbConnection>();
            var dbtran = Substitute.For<IDbTransaction>();
            dbhelper.BeginTransaction(Arg.Any<string>()).Returns(new Tuple<IDbConnection, IDbTransaction>(dbconn, dbtran));
            dbhelper.CommitTransaction(Arg.Any<IDbTransaction>()).Returns(true);
            var _dbcontext = DbContextFaker.Mock()
                 .Gives().AccountAdd(2)
                 .Gives().AccessAdd(access);
            _dbcontext.Accesss.SelectCodeAsync(Arg.Any<string>()).Returns(new HotDbException("Account Does not Exist","No Data"));
            _dbcontext.Addresses.UpdateAsync(Arg.Any<Address>(), Arg.Any<IDbConnection>(), Arg.Any<IDbTransaction>()).Returns(true);
            _dbcontext.AccessWebs.UpdateAsync(Arg.Any<AccessWeb>(), Arg.Any<IDbConnection>(), Arg.Any<IDbTransaction>()).Returns(true);

            var request = new AccountRegistrationCommand("BeavenAccount", "Beaven", "Guwa", "Beaven@Guwa", "thisIsMyPassoword123", "63-123-1234 Acf", "Norlin", accountType, "Bryan@hot.co.zw");
            var sut = new AccountRegistrationCommandHandler(_dbcontext, dbhelper, config);

            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            await _dbcontext.Received().Accesss.AddAsync(Arg.Is<Access>(a => a.AccessId == access.AccessId));
            await _dbcontext.Received().AccessWebs.UpdateAsync(Arg.Any<AccessWeb>());
            await _dbcontext.Received().Accounts.AddAsync(Arg.Any<Account>());

        }

        [Fact]
        public async Task AccountSearchQuery_ShouldReturnList_Test()
        {
            var logger = LoggerFaker.GetLogger<AccountSearchQueryHandler>();
            var request = new AccountSearchQuery("norlin");
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomAccountList(5);
            var sut = new AccountSearchQueryHandler(_dbcontext, logger);

            //Act
            var result = await sut.Handle(request, new CancellationToken());

            //Assert
            result.IsT0.Should().Be(true);
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Should().HaveCount(5);

            await _dbcontext.Received().Accounts.SearchAsync(Arg.Any<string>());
        }
        
        [Fact]
        public async Task ChangePassword_ShouldReturnBoolean_Test()
        {
            //Arrange
            var request = new ChangePasswordCommand("0774123456", "1234", "4562");
            var _dbcontext = DbContextFaker.Mock()
                .Gives().AccessSelectLogin()
                .Gives().AccessPasswordChange(true);
            var logger = LoggerFaker.GetLogger<ChangePasswordCommandHandler>();
            var sut = new ChangePasswordCommandHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.AsT0.Should().Be(true);
            await _dbcontext.Received().Accesss.SelectLoginAsync(Arg.Is<string>("0774123456"), Arg.Is<string>("1234"));
            await _dbcontext.Received().Accesss.PasswordChangeAsync(Arg.Any<Access>());
            //check if password has changed
        }
        [Fact]
        public async Task GetAccountByAccountId_ShouldReturnAccountDetailedModel_Test()
        {
            //Arrange
            var account = AccountDbFacker.GetSingle();
            var address = AccessDbFacker.GetAddressSingle();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().AccessSelectLogin()
                .Gives().Account(account)
                .Gives().RandomProfilesList(10)
                .Gives().AccessByAccountIdList(10)
                .Gives().Address(address);
            var accountModel = AccountDbFacker.GetAccountModelListTestData(account);
            mapper.Map<AccountDetailedModel>(Arg.Any<Account>()).Returns(accountModel);
            var logger = LoggerFaker.GetLogger<GetAccountByAccountIdQueryHandler>();
            var request = new GetAccountByAccountIdQuery(123456);
            var sut = new GetAccountByAccountIdQueryHandler(_dbcontext, mapper, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.AccountName.Should().Be(account.AccountName);
            await _dbcontext.Received().Accesss.SelectLoginAsync(Arg.Any<string>(), Arg.Any<string>());
            await _dbcontext.Received().Accounts.GetAsync(Arg.Any<int>());
            await _dbcontext.Received().Profiles.ListAsync();
            await _dbcontext.Received().Accesss.ListAsync(Arg.Any<long>());
            await _dbcontext.Received().Addresses.GetAsync(Arg.Any<int>());
        }

        [Fact]
        public async Task GetAccount_ShouldReturnAccount_Test()
        {
            //Arrange
            var account = AccountDbFacker.GetSingle();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().AccessSelectCode()
                .Gives().Account(account);
            var logger = LoggerFaker.GetLogger<GetAccountQueryHandler>();
            var request = new GetAccountQuery("0773404368");
            var sut = new GetAccountQueryHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.AccountID.Should().Be(account.AccountID);
            await _dbcontext.Received().Accesss.SelectCodeAsync(Arg.Any<string>());
            await _dbcontext.Received().Accounts.GetAsync(Arg.Any<int>());
        }
        [Fact]
        public async Task GetBalance_shouldReturn_Test()
        {
            //Arrange
            var account = AccountDbFacker.GetSingle();
            var _dbcontext = DbContextFaker.Mock()
               .Gives().Account(account);
            var logger = LoggerFaker.GetLogger<GetBalanceQueryHandler>();
            var request = new GetBalanceQuery(12356);
            var sut = new GetBalanceQueryHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.Should().Be(account.Balance);
            await _dbcontext.Received().Accounts.GetAsync(Arg.Any<int>());
        }
        [Fact]
        public async Task GetRecentAccountPayments_ShouldReturnList_Test()
        {
            //Arrange
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomPaymentsRecentList(12);
            var logger = LoggerFaker.GetLogger<GetRecentAccountPaymentsQueryHandler>();
            var request = new GetRecentAccountPaymentsQuery(123);
            var sut = new GetRecentAccountPaymentsQueryHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.Count.Should().Be(12);
            await _dbcontext.Received().Payments.ListRecentAsync(Arg.Any<int>());
        }

        [Fact]
        public async Task GetUSDBalance_ShouldReturnDecimalBalance_Test()
        {
            var account = AccountDbFacker.GetSingle();
            long accountId = 12345;
            var request = new GetUSDBalanceQuery(accountId);
            var _dbcontext = DbContextFaker.Mock()
                .Gives().Account(account);
            var logger = LoggerFaker.GetLogger<GetUSDBalanceQueryHandler>();
            var sut = new GetUSDBalanceQueryHandler(_dbcontext, logger);

            //Act
            var result = await sut.Handle(request, new CancellationToken());

            //Assert
            result.IsT0.Should().BeTrue();
            //result.AsT0.Should().Be(account.Balance);
            await _dbcontext.Received().Accounts.GetAsync(Arg.Any<long>());
        }


        [Fact]
        public async Task GetZesaBalance_ShouldreturnDecimal_Test()
        {
            //Arrange
            var account = AccountDbFacker.GetSingle();
            var _dbcontext = DbContextFaker.Mock()
               .Gives().Account(account);
            var logger = LoggerFaker.GetLogger<GetZesaBalanceQueryHandler>();
            var request = new GetZesaBalanceQuery(123563);
            var sut = new GetZesaBalanceQueryHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.Should().Be(account.ZesaBalance);
            await _dbcontext.Received().Accounts.GetAsync(Arg.Any<int>());
        }
        [Fact]
        public async Task LinkToExistingAccount_ShouldReturnLinkAccountResult_Test()
        {
            //Arrange
            var access = AccessDbFacker.GetSingle();
            var account = AccountDbFacker.GetSingle();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().Account(account)
                .Gives().AccessAdd(access);
            var logger = LoggerFaker.GetLogger<LinktoExistingAccountCommandHandler>();
            var request = new LinktoExisitngAccountCommand(123561, "0774123456", "4562");
            var sut = new LinktoExistingAccountCommandHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.AsT0.Success.Should().BeTrue();
            result.AsT0.AccountId.Should().Be(123561);
            await _dbcontext.Received().Accounts.GetAsync(Arg.Any<int>());
            await _dbcontext.Received().Accesss.AddAsync(Arg.Any<Access>());
        }
        [Fact]
        public async Task SearchAccountByFilter_ShouldReturnAccount_Test()
        {
            //Arrange
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomAccountList(10);
            var logger = LoggerFaker.GetLogger<SearchAccountByFilterHandler>();
            var request = new SearchAccountByFilterQuery("test filter");
            var sut = new SearchAccountByFilterHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.Count.Should().Be(10);
            await _dbcontext.Received().Accounts.SearchAsync(Arg.Any<string>());
        }
        [Fact]
        public async Task TransferWalletAirtimeBalance_ShouldReturnTransferResult_Test()
        {
            //Arrange
            var account = AccountDbFacker.GetSingle();
            var template = TemplateDbFacker.GetSingle();
            
            var _dbcontext = DbContextFaker.Mock()
                 .Gives().Template(template)
                 .Gives().PaymentAdd(12)
                 //.Gives().TransferAdd(5)
                 .Gives().Account(account)
                 .Gives().AccessSelectCode();
            var dbconn = Substitute.For<IDbConnection>();
            var dbtran = Substitute.For<IDbTransaction>();
            _dbcontext.BeginTransactionAsync().Returns(new Tuple<IDbConnection, IDbTransaction>(dbconn, dbtran));
            _dbcontext.CompleteTransaction(Arg.Any<IDbTransaction>()).Returns(true);
           
            _dbcontext.Transfers.AddAsync(Arg.Any<Transfer>(), Arg.Any<IDbConnection>(), Arg.Any<IDbTransaction>()).Returns(5);
            var request = new TransferWalletAirtimeBalanceCommand("0775085496", "0773404368", 125);
            var sut = new TransferWalletAirtimeBalanceCommandHandler(_dbcontext);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.Amount.Should().Be(125);
            result.AsT0.ReplyCode.Should().Be(ReplyCode.Success);
            await _dbcontext.Received().Templates.GetAsync(Arg.Any<int>());
            await _dbcontext.Received().Payments.AddAsync(Arg.Any<Payment>());
            await _dbcontext.Received().Accounts.GetAsync(Arg.Any<int>());
            await _dbcontext.Received().Accesss.SelectCodeAsync(Arg.Any<string>());
            await _dbcontext.Received().Transfers.AddAsync(Arg.Any<Transfer>(), Arg.Any<IDbConnection>(), Arg.Any<IDbTransaction>());
        } [Fact]
        public async Task TransferUSDWalletAirtimeBalance_ShouldReturnTransferResult_Test()
        {
            //Arrange
            var account = AccountDbFacker.GetSingle();
            var template = TemplateDbFacker.GetSingle();
           
            var _dbcontext = DbContextFaker.Mock()
                 .Gives().Template(template)
                 .Gives().PaymentAdd(12)
                 //.Gives().TransferAdd(5)
                 .Gives().Account(account)
                 .Gives().AccessSelectCode();
            var dbconn = Substitute.For<IDbConnection>();
            var dbtran = Substitute.For<IDbTransaction>();
            _dbcontext.BeginTransactionAsync().Returns(new Tuple<IDbConnection, IDbTransaction>(dbconn, dbtran));
            _dbcontext.CompleteTransaction(Arg.Any<IDbTransaction>()).Returns(true);
            _dbcontext.Transfers.AddAsync(Arg.Any<Transfer>(), Arg.Any<IDbConnection>(), Arg.Any<IDbTransaction>()).Returns(5);
            var request = new TransferWalletAirtimeBalanceCommand("0772304733", "0775430814", 135);
            var sut = new TransferWalletAirtimeBalanceCommandHandler(_dbcontext);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.Amount.Should().Be(135);
            result.AsT0.ReplyCode.Should().Be(ReplyCode.Success);
            await _dbcontext.Received().Templates.GetAsync(Arg.Any<int>());
            await _dbcontext.Received().Payments.AddAsync(Arg.Any<Payment>());
            await _dbcontext.Received().Accounts.GetAsync(Arg.Any<int>());
            await _dbcontext.Received().Accesss.SelectCodeAsync(Arg.Any<string>());
            await _dbcontext.Received().Transfers.AddAsync(Arg.Any<Transfer>(), Arg.Any<IDbConnection>(), Arg.Any<IDbTransaction>());
        }
       

        [Fact]
        public async Task UpdateAccountDetails_ShouldReturnAccount_Test()
        {
            //Arrange
            var account = AccountDbFacker.GetSingle();
            var _dbcontext = DbContextFaker.Mock()
               .Gives().Account(account)
               .Gives().AccountUpdate(true);
            var logger = LoggerFaker.GetLogger<UpdateAccountDetailsCommandHandler>();
            var request = new UpdateAccountDetailsCommand(account);
            var sut = new UpdateAccountDetailsCommandHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.Should().Be(true);
            await _dbcontext.Received().Accounts.GetAsync(Arg.Any<int>());
            await _dbcontext.Received().Accounts.UpdateAsync(Arg.Any<Account>());
        }
        [Fact]
        public async Task UpdateAccountDetails_ShouldReturnBoolen_Test()
        {
            var account = AccountDbFacker.GetSingle();
            var request = new UpdateAccountDetailsCommand(account);
            var _dbcontext = DbContextFaker.Mock()
                .Gives().Account(account)
                .Gives().AccountUpdate(true);
            var logger = LoggerFaker.GetLogger<UpdateAccountDetailsCommandHandler>();
            var sut = new UpdateAccountDetailsCommandHandler(_dbcontext, logger);

            //Act
            var result = await sut.Handle(request, new CancellationToken());

            //Assert
            result.IsT0.Should().BeTrue();

            await _dbcontext.Received().Accounts.GetAsync(Arg.Any<long>());
         
        }


    }
}