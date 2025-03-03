using OneOf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelOne.Application.Common.Exceptions;
using TelOne.Application.Common.Models;
using TelOne.Domain.Models;

namespace TelOne.Application.Common.Interfaces
{
    public interface ITelOneService
    {
        public Task<OneOf<CustomerAccountResponse, APIException, string>> VerifyCustomer(string AccountNumber);
        public Task<OneOf<QueryCustomerVoiceBalanceResponse, APIException, string>> QueryCustomerVoiceBalance(string CustomerAccount);
        public Task<OneOf<VoipRechargeResponse, APIException, string>> VoipRecharge(string CustomerAccount, decimal Amount, string Reference);
        public Task<OneOf<RechargeAdslAccountResponse, APIException, string>> RechargeAdslAccount(string CustomerAccount, int ProductId, string Reference);
        public Task<OneOf<RechargeAdslAccountResponse, APIException, string>> RechargeAdslAccountUSD(string CustomerAccount, int ProductId, string Reference);
        public Task<OneOf<PurchaseBroadbandProductsResponse, APIException, string>> PurchaseBroadbandProducts(int ProductId, int Quantity, string Reference);
        public Task<OneOf<List<BroadbandProduct>, APIException, string>> QueryBroadbandProducts();
        public Task<OneOf<List<BroadbandProduct>, APIException, string>> QueryBroadbandProductsUSD();
        public Task<OneOf<List<AccountBalance>, APIException, string>> QueryMerchantBalance();
        public Task<OneOf<PayBillResponse, APIException, string>> PayBill(string CustomerAccount, decimal Amount, string Reference);
        public Task<OneOf<List<MerchantTransaction>, APIException, string>> QueryMerchantTransactions(DateTime StartDate, DateTime EndDate);
        public Task<OneOf<List<MerchantLedgerTransaction>, APIException, string>> QueryMerchantLedgerTransactions(DateTime StartDate, DateTime EndDate);
    }
}
