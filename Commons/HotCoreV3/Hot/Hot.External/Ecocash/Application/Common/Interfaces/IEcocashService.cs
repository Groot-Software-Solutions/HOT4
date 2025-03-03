using Hot.Ecocash.Domain.Entities;
using Hot.Ecocash.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Ecocash.Application.Common.Interfaces
{
    public interface IEcocashService
    {
        public void SetOptions(ServiceOptions options,string APIName);
        public EcocashResult ChargeNumber(string MobileNumber, string Reference, decimal Amount);
        public EcocashResult ChargeNumber(string MobileNumber, string Reference, decimal Amount, string OnBehalfOf);
        public EcocashResult ChargeNumber(string MobileNumber, string Reference, decimal Amount, string OnBehalfOf, Currencies currency); 
        public EcocashResult ChargeNumber(string MobileNumber, string Reference, decimal Amount, string OnBehalfOf,string Remark); 
        public EcocashResult RefundTransaction(string MobileNumber, string Reference, decimal Amount, string EcocashReference);
        public EcocashResult QueryTransaction(string MobileNumber, string Reference);
        public EcocashResult ListTransactions(string MobileNumber);
        public Task<EcocashResult> ChargeNumberAsync(string MobileNumber, string Reference, decimal Amount, string onBehalfOf, string remark = "HotRecharge", Currencies currency= Currencies.ZiG);
        public Task<EcocashResult> RefundTransactionAsync(string MobileNumber, string Reference, decimal Amount, string EcocashReference);
        public Task<EcocashResult> RefundTransactionAsync(string MobileNumber, string Reference, decimal Amount, string EcocashReference, Currencies currency);
        public Task<EcocashResult> QueryTransactionAsync(string MobileNumber, string Reference);
        public Task<EcocashResult> ListTransactionsAsync(string MobileNumber);
    }
}
