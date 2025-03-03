using Bogus;
using FluentAssertions;
using Hot.Application.Common.Extensions;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using Hot.Ecocash.Application.Actions;
using Hot.Ecocash.Application.Common.Extensions;
using Hot.Ecocash.Application.Common.Interfaces;
using Hot.Ecocash.Application.Common.Models;
using Hot.Ecocash.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Hot.Ecocash.Application.Actions.CompleteEcocashCommand; 

namespace Hot.Ecocash.Tests.Extensions
{
    public class EcocashTransactionExtensionsTests
    {
        private readonly IDbContext _dbcontext =
           Substitute.For<IDbContext>();
        private readonly IEcocashServiceFactory ecocash =
           Substitute.For<IEcocashServiceFactory>();

        private readonly IMediator _mediator =
          Substitute.For<IMediator>();
        private readonly IAPIService apiService =
          Substitute.For<IAPIService>();
        private readonly IHttpClientFactory httpFactory =
         Substitute.For<IHttpClientFactory>();

        //[Fact]
        //public async Task GetTemplate_ShouldReturnTemplate_TestAsync()
        //{
        //    var logger = Substitute.For<ILogger<SelfTopUpCommandHandler>>();
        //    //Arrange
        //    // IsAPITransaction() returns false

        //    var bankTrx = GenerateBankTrxTestData();
        //    var template = GenerateTemplateTestDate();
        //    _dbcontext.Templates.GetAsync(Arg.Any<int>()).Returns(template);
        //    var sut = new SelfTopUpCommandHandler(_dbcontext, logger, ecocash);
        //    //Act
        //    var result = await sut.GetTemplateAsync();
        //    //Assert
        //    result.Should().Be(template);

        //}
        //[Fact]
        //public async Task GetSMSText_ShouldReturnSMSText_TEstAsync()
        //{
        //    var logger = Substitute.For<ILogger<SelfTopUpCommandHandler>>();
        //    //Arrange
        //    var access = GenerateAccessTestData();
        //    var template = GenerateTemplateTestDate();
        //    var sut = new SelfTopUpCommandHandler(_dbcontext, logger, ecocash);
        //    //Act
        //    //var result = await sut.Get(access,template);
        //    //Assert

        //}
        [Fact]
        public async Task SendingNotificationSMS_TestAsync()
        {
            var logger = Substitute.For<ILogger<CompleteEcocashCommandHandler>>(); 
            //Arrange
            var banktrx = GenerateBankTrxTestData();
            var access = GenerateAccessTestData();
            var account = GenerateAccountTestData();
            var payment = GeneratePaymentTestData();
            var transaction = GenerateTransactionTestData();
            
            var template = GenerateTemplateTestData(); 
            CompleteEcocashCommand request = new(transaction) { Item = transaction, LastUser = "LastUser" }; 
            _dbcontext.Templates.Get(Arg.Any<int>()).Returns(template);
             
            var sut = new CompleteEcocashCommandHandler(_dbcontext, logger, _mediator, ecocash);

            //Act
             await sut.SendNotificationSMS(request, banktrx, access, account, payment);
            //Assert
            await _dbcontext.Received().SMSs.AddAsync(Arg.Any<SMS>());
        }

        private Template GenerateTemplateTestData()
        {
            var templateFaker = new Faker<Template>()
                .RuleFor(a => a.TemplateName, f => f.Random.String())
                .RuleFor(a => a.TemplateId, f => f.Random.Int())
                .RuleFor(a => a.TemplateText, f => f.Random.String());
            return templateFaker.Generate();                
        }

        private SMS GenerateSMSTestData()
        {
            var smsfaker = new Faker<SMS>()
            .RuleFor(a => a.SmppID, f => f.Random.Int())
            .RuleFor(a => a.SMSID, f => f.Random.Long())
            .RuleFor(a => a.Direction, f => f.Random.Bool())
            .RuleFor(a => a.Mobile, f => f.Random.String())
            .RuleFor(a => a.SMSText, f => f.Random.String());
            return smsfaker.Generate();

        }

        private Template GenerateTemplateTestDate()
        {
            var templatefaker = new Faker<Template>()
                .RuleFor(a => a.TemplateId, f => f.Random.Int())
                .RuleFor(a => a.TemplateName, f => f.Name.ToString())
                .RuleFor(a => a.TemplateText, f => f.ToString());
            return templatefaker.Generate();

        }

        public static BankTrx GenerateBankTrxTestData()
        {
            DateTime startDate = Convert.ToDateTime("01-01-2008");
            var bankTrxfaker = new Faker<BankTrx>()
                .RuleFor(a => a.BankTrxID, f => f.Random.Long())
                .RuleFor(a => a.BankTrxBatchID, f => f.Random.Long())
                .RuleFor(a => a.BankTrxTypeID, f => f.Random.Byte())
            .RuleFor(a => a.BankTrxStateID, f => f.Random.Byte())
                .RuleFor(a => a.Amount, f => f.Random.Decimal())
                .RuleFor(a => a.TrxDate, f => f.Date.Between(startDate, DateTime.Now))
            .RuleFor(a => a.Identifier, f => f.Random.ToString())
                .RuleFor(a => a.RefName, f => f.Name.ToString())
                .RuleFor(a => a.Branch, f => f.Random.ToString())
            .RuleFor(a => a.BankRef, f => f.Random.ToString())
                .RuleFor(a => a.Balance, f => f.Random.Decimal())
                .RuleFor(a => a.PaymentID, f => f.Random.Long());

            return bankTrxfaker.Generate();
        }
        public static Transaction GenerateTransactionTestData()
        {
            var transactionfaker = new Faker<Transaction>()
                .RuleFor(a => a.clientCorrelator, f => f.Random.ToString())
                .RuleFor(a => a.endTime, f => f.Random.Long())
                .RuleFor(a => a.startTime, f => f.Random.Long())
                .RuleFor(a => a.notifyUrl, f => f.Random.ToString())
                .RuleFor(a => a.referenceCode, f => f.Random.ToString())
                .RuleFor(a => a.endUserId, f => f.Random.ToString())
                .RuleFor(a => a.serverReferenceCode, f => f.Random.ToString())
                .RuleFor(a => a.transactionOperationStatus, f => f.Name.ToString())
                //.RuleFor(a => a.paymentAmount, f => f.Random.Decimal())
                .RuleFor(a => a.ecocashReference, f => f.Random.ToString())
                .RuleFor(a => a.merchantCode, f => f.Random.ToString())
                .RuleFor(a => a.merchantPin, f => f.Random.ToString())
                .RuleFor(a => a.merchantNumber, f => f.Random.ToString())
                .RuleFor(a => a.notificationFormat, f => f.Random.ToString())
                .RuleFor(a => a.originalServerReferenceCode, f => f.Random.ToString());

            return transactionfaker.Generate();
        }
        public static Account GenerateAccountTestData()
        {
            DateTime startDate = Convert.ToDateTime("01-01-2008");
            var accountfaker = new Faker<Account>()
                .RuleFor(a => a.AccountID, f => f.Random.Long())
                .RuleFor(a => a.ProfileID, f => f.Random.Int())
                .RuleFor(a => a.AccountName, f => f.Name.ToString())
                .RuleFor(a => a.NationalID, f => f.Random.ToString())
                .RuleFor(a => a.Email, f => f.Random.ToString())
                .RuleFor(a => a.ReferredBy, f => f.Random.ToString())
                .RuleFor(a => a.InsertDate, f => f.Date.Between(startDate, DateTime.Now))
                .RuleFor(a => a.Balance, f => f.Random.Decimal())
                //.RuleFor(a => a.paymentAmount, f => f.Random.Decimal())
                .RuleFor(a => a.SaleValue, f => f.Random.Decimal())
                .RuleFor(a => a.ZesaBalance, f => f.Random.Decimal());

            return accountfaker.Generate();
        }
        public static Payment GeneratePaymentTestData()
        {
            DateTime startDate = Convert.ToDateTime("01-01-2008");
            var paymentfaker = new Faker<Payment>()
                .RuleFor(a => a.PaymentId, f => f.Random.Long())
                .RuleFor(a => a.AccountId, f => f.Random.Long())
                .RuleFor(a => a.PaymentTypeId, f => f.Random.Byte())
                .RuleFor(a => a.PaymentSourceId, f => f.Random.Byte())
                .RuleFor(a => a.Amount, f => f.Random.Decimal())
                .RuleFor(a => a.PaymentDate, f => f.Date.Between(startDate, DateTime.Now))
                .RuleFor(a => a.Reference, f => f.Random.ToString())
                .RuleFor(a => a.LastUser, f => f.Random.ToString());
            return paymentfaker.Generate();
        }
        public static Access GenerateAccessTestData()
        {
            DateTime startDate = Convert.ToDateTime("01-01-2008");
            var accessFaker = new Faker<Access>()
                .RuleFor(a => a.AccessId, f => f.Random.Long())
                .RuleFor(a => a.AccessCode, f => f.Random.String())
                .RuleFor(a => a.ChannelID, f => f.Random.Byte())
                .RuleFor(a => a.Deleted, f => f.Random.Bool())
                .RuleFor(a => a.PasswordHash, f => f.Random.String())
                .RuleFor(a => a.PasswordSalt, f => f.Random.String())
                .RuleFor(a => a.InsertDate, f => f.Date.Between(startDate, DateTime.Now))
                .RuleFor(a => a.AccessPassword, f => f.Random.String());

            return accessFaker.Generate();
        }
    }
}
