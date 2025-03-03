using Bogus;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Models; 
using NSubstitute;
using System;
using System.Collections.Generic; 

namespace Hot.Tests.Common.Fakers.DbContext.Tables
{

    public static class ReportsDbFacker
    {   
        public static IDbContext RandomEconetStatsList(this DbFakerService service, int count = 5)
        {
            service.dbContext.Report.GetEconetStats(Arg.Any<DateTime>(), Arg.Any<DateTime>()).Returns(GetList(count));
            service.dbContext.Report.GetEconetStatsAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>()).Returns(GetList(count));
            return service.dbContext;
        }
        public static IDbContext RandomPaymentList(this DbFakerService service, int count = 5)
        {
            service.dbContext.Report.GetPayments(Arg.Any<DateTime>(), Arg.Any<DateTime>(),Arg.Any<int>(), Arg.Any<long>(), Arg.Any<int>(), Arg.Any<int>()).Returns(GetPaymentsList(count));
            service.dbContext.Report.GetPaymentsAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>(),Arg.Any<int>(), Arg.Any<long>(), Arg.Any<int>(), Arg.Any<int>()).Returns(GetPaymentsList(count));
            return service.dbContext;
        }
        public static IDbContext RandomProfileDiscountList(this DbFakerService service, int count = 5)
        {
           
            service.dbContext.Report.GetProfileDiscounts(Arg.Any<int>(), Arg.Any<int>()).Returns(GetProfileDiscountsList(count));
            service.dbContext.Report.GetProfileDiscountsAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(GetProfileDiscountsList(count));
            return service.dbContext;
        }
        public static IDbContext RandomPeriodicStatsList(this DbFakerService service, int count = 5)
        {
            service.dbContext.Report.GetStats(Arg.Any<DateTime>(), Arg.Any<DateTime>(),Arg.Any<int>(), Arg.Any<long>(), Arg.Any<int>()).Returns(GetPeriodicStatsList(count));
            service.dbContext.Report.GetStatsAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>(),Arg.Any<int>(), Arg.Any<long>(), Arg.Any<int>()).Returns(GetPeriodicStatsList(count));

            return service.dbContext;
        }
        public static IDbContext RandomStatementTransactionList(this DbFakerService service, int count = 5)
        {
            service.dbContext.Report.GetTransactions(Arg.Any<DateTime>(), Arg.Any<DateTime>(),Arg.Any<int>(), Arg.Any<long>(), Arg.Any<int>(), Arg.Any<int>()).Returns(GetStatementTransactionList(count));
            service.dbContext.Report.GetTransactionsAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<int>(), Arg.Any<long>(), Arg.Any<int>(), Arg.Any<int>()).Returns(GetStatementTransactionList(count));

            return service.dbContext;
        }
        public static IDbContext RandomsactionList(this DbFakerService service, int count = 5)
        {
            service.dbContext.Report.GetTransactions(Arg.Any<DateTime>(), Arg.Any<DateTime>(),Arg.Any<int>(), Arg.Any<long>(), Arg.Any<int>(), Arg.Any<int>()).Returns(GetStatementTransactionList(count));
            service.dbContext.Report.GetTransactionsAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<int>(), Arg.Any<long>(), Arg.Any<int>(), Arg.Any<int>()).Returns(GetStatementTransactionList(count));

            return service.dbContext;
        }
        public static IDbContext RandomStatement(this DbFakerService service, int count = 5)
        {
           
            service.dbContext.Report.GetStatement(Arg.Any<long>(),Arg.Any<DateTime>(), Arg.Any<DateTime>()).Returns(GetStatementTransactionList(count));
            service.dbContext.Report.GetStatementAsync(Arg.Any<long>(),Arg.Any<DateTime>(), Arg.Any<DateTime>()).Returns(GetStatementTransactionList(count));

            return service.dbContext;
        }
        public static IDbContext StartingBalance(this DbFakerService service, decimal Balance)
        {
         
            service.dbContext.Report.GetStartingBalance(Arg.Any<long>(), Arg.Any<DateTime>()).Returns(Balance);
            service.dbContext.Report.GetStartingBalanceAsync(Arg.Any<long>(), Arg.Any<DateTime>()).Returns(Balance);
            return service.dbContext;
        } 
      

        



        public static EconetStatsResult GetSingle()
        {
            return GetFaker().Generate();
        }
        public static List<EconetStatsResult> GetList(int count)
        {
            return GetFaker().Generate(count);
        }
        public static List<PaymentResult> GetPaymentsList(int count)
        {
            return GetPaymentFaker().Generate(count);
        }
        public static List<ProfileDiscountResult> GetProfileDiscountsList(int count)
        {
            return GetProfileDiscountsFaker().Generate(count);
        }
        public static List<PeriodicStatsResult> GetPeriodicStatsList(int count)
        {
            return GetPeriodicStatsFaker().Generate(count);
        }
        public static List<StatementTransaction> GetStatementTransactionList(int count)
        {
            return GetStatementTransactionFaker().Generate(count);
        }

        private static Faker<EconetStatsResult> GetFaker()
        {
            var reportsfaker = new Faker<EconetStatsResult>()

                .RuleFor(a => a.Platform, f => f.Name.ToString())
                .RuleFor(a => a.TranCount, f => f.Random.Int())
                .RuleFor(a => a.TotalAmount, f => f.Random.Decimal());

            return reportsfaker;
        }
        public static Faker<PaymentResult> GetPaymentFaker()
        {
            DateTime startDate = Convert.ToDateTime("01-01-2008");
            DateTime endDate = DateTime.Now;
            var paymentsfaker = new Faker<PaymentResult>()
                .RuleFor(a => a.PaymentID, f => f.Random.Long())
                .RuleFor(a => a.AccountID, f => f.Random.Long())
                .RuleFor(a => a.AccountName, f => f.Name.ToString())
                .RuleFor(a => a.PaymentType, f => f.Name.ToString())
                .RuleFor(a => a.Amount, f => f.Random.Decimal())
                .RuleFor(a => a.PaymentDate, f => f.Date.Between(startDate, endDate))
                .RuleFor(a => a.Reference, f => f.Name.ToString())
                .RuleFor(a => a.MyProperty, f => f.Name.ToString());


            return paymentsfaker;
        }
        public static Faker<ProfileDiscountResult> GetProfileDiscountsFaker()
        {
         
            var profileDiscountfaker = new Faker<ProfileDiscountResult>()
                .RuleFor(a => a.ProfileName, f => f.Name.ToString())
                .RuleFor(a => a.ProfileDiscountID, f => f.Random.Int())
                .RuleFor(a => a.ProfileId, f => f.Random.Int())
                .RuleFor(a => a.BrandID, f => f.Random.Int())
                .RuleFor(a => a.Discount, f => f.Random.Decimal())
                .RuleFor(a => a.NetworkID, f => f.Random.Int())
                .RuleFor(a => a.Network, f => f.Name.ToString())
                .RuleFor(a => a.BrnadName, f => f.Name.ToString());
            return profileDiscountfaker;
        }
        public static Faker<PeriodicStatsResult> GetPeriodicStatsFaker()
        {

            var periodicStatsfaker = new Faker<PeriodicStatsResult>()
                .RuleFor(a => a.Network, f => f.Name.ToString())
                .RuleFor(a => a.Period, f => f.Random.Int())
                .RuleFor(a => a.Count, f => f.Random.Int())
                .RuleFor(a => a.Amount, f => f.Random.Decimal());

            return periodicStatsfaker;
        }
        public static Faker<PeriodicStatsResult> GetTransactionFaker()
        {

            var periodicStatsfaker = new Faker<PeriodicStatsResult>()
                .RuleFor(a => a.Network, f => f.Name.ToString())
                .RuleFor(a => a.Period, f => f.Random.Int())
                .RuleFor(a => a.Count, f => f.Random.Int())
                .RuleFor(a => a.Amount, f => f.Random.Decimal());

            return periodicStatsfaker;
        }

        public static Faker<StatementTransaction> GetStatementTransactionFaker()
        {
            DateTime startDate = Convert.ToDateTime("01-01-2008");
            DateTime endDate = DateTime.Now;

            var statementTransactionfaker = new Faker<StatementTransaction>()
  
                .RuleFor(a => a.RechargeDate, f => f.Date.Between(startDate, endDate))
                .RuleFor(a => a.RechargeID, f => f.Random.Int())
                .RuleFor(a => a.TranType, f => f.Name.ToString())
                .RuleFor(a => a.AccessCode, f => f.Name.ToString())
                .RuleFor(a => a.Discount, f => f.Random.Decimal())
                .RuleFor(a => a.Amount, f => f.Random.Decimal())
                .RuleFor(a => a.Cost, f => f.Random.Decimal())
                .RuleFor(a => a.Mobile, f => f.Name.ToString());

            return statementTransactionfaker;
        }       
        public static Faker<StatementTransactionModel> GetStatementTransactionModelFaker()
        {
            DateTime startDate = Convert.ToDateTime("01-01-2008");
            DateTime endDate = DateTime.Now;

            var statementTransactionModelfaker = new Faker<StatementTransactionModel>()
  
                .RuleFor(a => a.RechargeDate, f => f.Date.Between(startDate, endDate))
                .RuleFor(a => a.RechargeID, f => f.Random.Int())
                .RuleFor(a => a.TranType, f => f.Name.ToString())
                .RuleFor(a => a.AccessCode, f => f.Name.ToString())
                .RuleFor(a => a.Discount, f => f.Random.Decimal())
                .RuleFor(a => a.Amount, f => f.Random.Decimal())
                .RuleFor(a => a.Cost, f => f.Random.Decimal())
                .RuleFor(a => a.Mobile, f => f.Name.ToString());

            return statementTransactionModelfaker;
        }


    }

}
