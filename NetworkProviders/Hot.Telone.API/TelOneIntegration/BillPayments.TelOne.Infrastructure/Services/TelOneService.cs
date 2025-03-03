using MediatR;
using Microsoft.Extensions.Configuration;
using OneOf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelOne.Application.Accounts;
using TelOne.Application.AdslAccount;
using TelOne.Application.BroadbandProducts;
using TelOne.Application.Commands.Accounts;
using TelOne.Application.Commands.AdslAccount;
using TelOne.Application.Commands.BroadbandProducts;
using TelOne.Application.Commands.Merchant;
using TelOne.Application.Commands.Voip;
using TelOne.Application.Common.Exceptions;
using TelOne.Application.Common.Interfaces;
using TelOne.Application.Common.Models;
using TelOne.Domain.Enum;
using TelOne.Domain.Models;

namespace TelOne.Infrastructure.Services
{
    public class TelOneService : ITelOneService
    {
        private readonly IConfiguration _config;
        private readonly IMediator Mediator;

        public TelOneService(IConfiguration config, IMediator mediator)
        {
            _config = config;
            Mediator = mediator;
        }

        public async Task<OneOf<PayBillResponse, APIException, string>> PayBill
            (string CustomerAccount, decimal Amount, string Reference)
        {
            return await Mediator.Send(new PayBillCommand(
                new PayBillRequest
                {
                    APIKey = _config["APIKey"],
                    AccountSid = _config["AccountSid"],
                    BillAmount = Amount,
                    MerchantReference = Reference,
                    Currency = CurrencyCodes.ZiG,
                    Identifier = CustomerAccount,
                    PaymentMethod = "CommShop Payment",
                    //PaymentMethodData = "Comm Shop Payment",
                    Reason = "HotRecharge Bill Payment",
                }));
        }

        public async Task<OneOf<PurchaseBroadbandProductsResponse, APIException, string>> PurchaseBroadbandProducts
            (int ProductId, int Quantity, string Reference)
        {
            return await Mediator.Send(new PurchaseBroadbandProductsCommand(
                 new PurchaseBroadbandProductsRequest
                 {
                     APIKey = _config["APIKey"],
                     AccountSid = _config["AccountSid"],
                     MerchantReference = Reference,
                     OrderProducts = new List<OrderProduct>()
                     {
                        new OrderProduct()
                        {
                            ProductId=ProductId,
                            Quantity=Quantity
                        }
                     },
                     Currency = CurrencyCodes.ZWG,
                 }));
        }

        public async Task<OneOf<List<BroadbandProduct>, APIException, string>> QueryBroadbandProducts
            ()
        {
            return await Mediator.Send(new QueryBroadbandProductsCommand(
                new QueryBroadbandProductsRequest
                {
                    APIKey = _config["APIKey"],
                    AccountSid = _config["AccountSid"],
                }));
        }

        public async Task<OneOf<List<BroadbandProduct>, APIException, string>> QueryBroadbandProductsUSD()
        {
            return await Mediator.Send(new QueryBroadbandProductsCommandUSD(
            new QueryBroadbandProductsRequest
            {
                APIKey = _config["APIKey"],
                AccountSid = _config["AccountSid"],
            }));
        }

        public async Task<OneOf<QueryCustomerVoiceBalanceResponse, APIException, string>> QueryCustomerVoiceBalance
            (string CustomerAccount)
        {
            return await Mediator.Send(new QueryCustomerVoiceBalanceCommand(
                new QueryCustomerVoiceBalanceRequest
                {
                    MSISDN = CustomerAccount,
                    APIKey = _config["APIKey"],
                    AccountSid = _config["AccountSid"]
                }));
        }

        public async Task<OneOf<List<AccountBalance>, APIException, string>> QueryMerchantBalance
            ()
        {
            return await Mediator.Send(new QueryMerchantBalanceCommand(
                new Request
                {
                    APIKey = _config["APIKey"],
                    AccountSid = _config["AccountSid"],
                }));
        }

        public async Task<OneOf<List<MerchantLedgerTransaction>, APIException, string>> QueryMerchantLedgerTransactions
            (DateTime StartDate, DateTime EndDate)
        {
            return await Mediator.Send(new QueryMerchantTransactionsLedgerCommand(
                 new QueryMerchantTransactionsRequest
                 {
                     APIKey = _config["APIKey"],
                     AccountSid = _config["AccountSid"],
                     startDate = StartDate,
                     endDate = EndDate,
                 }));
        }

        public async Task<OneOf<List<MerchantTransaction>, APIException, string>> QueryMerchantTransactions
          (DateTime StartDate ,DateTime EndDate)
        {
            return await Mediator.Send(new QueryMerchantTransactionsCommand(
                new QueryMerchantTransactionsRequest
                {
                    APIKey = _config["APIKey"],
                    AccountSid = _config["AccountSid"],
                    startDate = StartDate,
                    endDate= EndDate,
                }));
        }

        public async Task<OneOf<RechargeAdslAccountResponse, APIException, string>> RechargeAdslAccount
            (string CustomerAccount, int ProductId, string Reference)
        {
            return await Mediator.Send(new RechargeAdslAccountCommand(
                new RechargeAdslAccountRequest
                {
                    APIKey = _config["APIKey"],
                    AccountSid = _config["AccountSid"],
                    MerchantReference = Reference,
                    ProductId = ProductId,
                    CustomerAccount = CustomerAccount,
                    Currency = CurrencyCodes.ZWG,
                }));
        }
        public async Task<OneOf<RechargeAdslAccountResponse, APIException, string>> RechargeAdslAccountUSD
    (string CustomerAccount, int ProductId, string Reference)
        {
            return await Mediator.Send(new RechargeAdslUSDAccountCommand(
                new RechargeAdslAccountRequest
                {
                    APIKey = _config["APIKey"],
                    AccountSid = _config["AccountSid"],
                    MerchantReference = Reference,
                    ProductId = ProductId,
                    CustomerAccount = CustomerAccount,
                    Currency = CurrencyCodes.USD,
                }));
        }

        public async Task<OneOf<CustomerAccountResponse, APIException, string>> VerifyCustomer
            (string AccountNumber)
        {
            return await Mediator.Send(new VerifyCustomerAccountCommand(
                new CustomerAccountRequest
                {
                    CustomerAccount = AccountNumber,
                    APIKey = _config["APIKey"],
                    AccountSid = _config["AccountSid"]
                }));
        }

        public async Task<OneOf<VoipRechargeResponse, APIException, string>> VoipRecharge(string CustomerAccount, decimal Amount, string Reference)
        {
            return await Mediator.Send(new VoipRechargeCommand(
               new VoipRechargeRequest
               {
                   APIKey = _config["APIKey"],
                   AccountSid = _config["AccountSid"],
                   MerchantReference = Reference,
                   CustomerAccount = CustomerAccount,
                   Currency = CurrencyCodes.ZWG,
                   VoiceAmount = Amount,
               }));
        }
    }
}
