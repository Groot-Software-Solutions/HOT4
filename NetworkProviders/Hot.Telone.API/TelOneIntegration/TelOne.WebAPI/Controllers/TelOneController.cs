using TelOne.Application.Accounts;
using TelOne.Application.AdslAccount;
using TelOne.Application.BroadbandProducts;
using TelOne.Application.Common.Models;
using TelOne.Domain.Enum;
using TelOne.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelOne.Application.Common.Interfaces;
using System;

namespace TelOne.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelOneController : ApiController
    {
        private readonly ILogger<TelOneController> _logger;
        private readonly ITelOneService _service;

        public TelOneController(ILogger<TelOneController> logger, ITelOneService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("VerifyCustomerAccount")]
        public async Task<ActionResult<CustomerAccountResponse>> VerifyCustomerAccount
            (string customerAccount)
        {
            var response = await _service
                .VerifyCustomer(customerAccount);

            return response.Match(
                item => Ok(item),
                error => StatusCode(503, error.Message),
                data => BadRequest(data)
                );
        }

        [HttpGet("RechargeAdslAccount")]
        public async Task<ActionResult<RechargeAdslAccountResponse>> RechargeAdslAccount
            (string reference, string customerAccount, int productId)
        {
            var response = await _service
                .RechargeAdslAccount(customerAccount, productId, reference);

            return response.Match(
                item => Ok(item),
                error => StatusCode(503, error.Message),
                data => BadRequest(data)
                );
        }

        [HttpGet("RechargeAdslAccountUSD")]
        public async Task<ActionResult<RechargeAdslAccountResponse>> RechargeAdslAccountUSD
            (string reference, string customerAccount, int productId)
        {
            var response = await _service
                .RechargeAdslAccountUSD(customerAccount, productId, reference);

            return response.Match(
                item => Ok(item),
                error => StatusCode(503, error.Message),
                data => BadRequest(data)
                );
        }

        [HttpGet("PurchaseBroadProducts")]
        public async Task<ActionResult<PurchaseBroadbandProductsResponse>> PurchaseBroadProducts
            (string reference, int productId, int quantity)
        {
            var response = await _service
                  .PurchaseBroadbandProducts(productId, quantity, reference);

            return response.Match(
                item => Ok(item),
                error => StatusCode(503, error.Message),
                data => BadRequest(data)
                );
        }

        [HttpGet("QueryBroadbandProducts")]
        public async Task<ActionResult<List<BroadbandProduct>>> QueryBroadbandProducts()
        {
            var response = await _service
                .QueryBroadbandProducts();

            return response.Match(
                item => Ok(item),
                error => StatusCode(503, error.Message),
                data => BadRequest(data)
                );
        }

        [HttpGet("QueryBroadbandProductsUSD")]
        public async Task<ActionResult<List<BroadbandProduct>>> QueryBroadbandProductsUSD()
        {
            var response = await _service
                .QueryBroadbandProductsUSD();

            return response.Match(
                item => Ok(item),
                error => StatusCode(503, error.Message),
                data => BadRequest(data)
                );
        }

        [HttpGet("PayBill")]
        public async Task<ActionResult<PayBillResponse>> PayBill
            (string CustomerAccount, decimal Amount, string Reference)
        {
            var response = await _service
                  .PayBill(CustomerAccount, Amount, Reference);

            return response.Match(
                item => Ok(item),
                error => StatusCode(503, error.Message),
                data => BadRequest(data)
                );
        }

        [HttpGet("QueryMerchantBalance")]
        public async Task<ActionResult<List<AccountBalance>>> QueryMerchantBalance
            ()
        {
            var response = await _service.QueryMerchantBalance();

            return response.Match(
                item => Ok(item),
                error => StatusCode(503, error.Message),
                data => BadRequest(data)
                );
        }


        [HttpGet("QueryMerchantTransactions")]
        public async Task<ActionResult<List<MerchantTransaction>>> QueryMerchantTransactions
            (DateTime startdate, DateTime enddate)
        {
            var response = await _service.QueryMerchantTransactions(startdate,enddate);

            return response.Match(
                list => Ok(list),
                error => StatusCode(503, error.Message),
                data => BadRequest(data)
                );
        }


        [HttpGet("QueryMerchantLedgerTransactions")]
        public async Task<ActionResult<List<MerchantTransaction>>> QueryMerchantLedgerTransactions
            (DateTime startdate, DateTime enddate)
        {
            var response = await _service.QueryMerchantLedgerTransactions(startdate, enddate);

            return response.Match(
                list => Ok(list),
                error => StatusCode(503, error.Message),
                data => BadRequest(data)
                );
        }

        [HttpGet("RechargeVoip")]
        public async Task<ActionResult<VoipRechargeResponse>> RechargeVoipAccount
           (string CustomerAccount, decimal Amount, string Reference)
        {
            var response = await _service
                  .VoipRecharge(CustomerAccount, Amount, Reference);

            return response.Match(
                item => Ok(item),
                error => StatusCode(503, error.Message),
                data => BadRequest(data)
                );
        }

        [HttpGet("CustomerVoiceBalance")]
        public async Task<ActionResult<QueryCustomerVoiceBalanceResponse>> QueryCustomerBalance
        (string customerAccount)
        {
            var response = await _service
                 .QueryCustomerVoiceBalance(customerAccount);

            return response.Match(
                item => Ok(item),
                error => StatusCode(503, error.Message),
                data => BadRequest(data)
                );
        }


    }
}
