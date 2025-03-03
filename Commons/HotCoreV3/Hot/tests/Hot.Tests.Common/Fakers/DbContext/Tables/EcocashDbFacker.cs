using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Tests.Common.Fakers.DbContext.Tables
{
    public class EcocashDbFacker
    {

        //public static Transaction GetTransactionSingle()
        //{
        //    return GetFaker().Generate();
        //}

        //public static List<Transaction> GetList(int count)
        //{
        //    return GetFaker().Generate(count);
        //}
        //public static FundWalletRequest GetFundWalletRequest()
        //{
        //    return FundWalletRequestFaker().Generate();
        //}
        //public static EcocashResult GetEcocashResult()
        //{
        //    return EcocashResultFaker().Generate();
        //}
        //private static Faker<EcocashResult> EcocashResultFaker()
        //{
        //    var ecocashResultFaker = new Faker<EcocashResult>()
        //        .RuleFor(a => a.ValidResponse, true)
        //        .RuleFor(a => a.Item, GetTransactionSingle());
        //    return ecocashResultFaker;
        //}

        //private static Faker<FundWalletRequest> FundWalletRequestFaker()
        //{
        //    var fundWalletRequestFaker = new Faker<FundWalletRequest>()
        //        .RuleFor(a => a.TargetMobile, "0773404368")
        //        .RuleFor(a => a.AccessCode, "hundirwanorlin@gmail.com")
        //        .RuleFor(a => a.Amount, 5000)
        //        .RuleFor(a => a.ReferenceId, "125632")
        //        .RuleFor(a => a.Remarks, "Test")
        //        .RuleFor(a => a.OnBehalfOf, "Norlin")
        //        .RuleFor(a => a.EcocashAccount, f => f.Random.Byte());
        //    return fundWalletRequestFaker;
        //}

        //private static Faker<Transaction> GetFaker()
        //{
        //    var ecocashTransactionFaker = new Faker<Transaction>()
        //        .RuleFor(a => a.clientCorrelator, f => f.Random.String())
        //        .RuleFor(a => a.notifyUrl, "Hot/Response")
        //        .RuleFor(a => a.referenceCode, "Hot")
        //        .RuleFor(a => a.endUserId, "12345U")
        //        .RuleFor(a => a.serverReferenceCode, "serverCode")
        //        .RuleFor(a => a.transactionOperationStatus, "Success")
        //        .RuleFor(a => a.paymentAmount, new PaymentAmountResponse())
        //        .RuleFor(a => a.ecocashReference, "Ecocash")
        //        .RuleFor(a => a.merchantCode, "011563")
        //        .RuleFor(a => a.merchantPin, "1234")
        //        .RuleFor(a => a.merchantNumber, "0773404368")
        //        .RuleFor(a => a.notificationFormat, "")
        //        .RuleFor(a => a.originalServerReferenceCode, "0000")
        //        .RuleFor(a => a.id, 123)
        //        .RuleFor(a => a.version, 12);
        //    return ecocashTransactionFaker;
        //}

    }
}
