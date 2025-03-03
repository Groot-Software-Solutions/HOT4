using System;
using System.Text.Json;
using System.Threading.Tasks;
using BillPayments.Application.Commands;
using BillPayments.Application.Zetdc.Tokens;
using BillPayments.Application.Services;
using BillPayments.Domain.Enums;
using BillPayments.Domain.Helpers;
using BillPayments.Domain.Models;
using BillPayments.Domain.Models.PurchaseToken;
using BillPayments.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PurchaseTokenRequest = BillPayments.Domain.Models.PurchaseTokenRequest;
using BillPayments.Application.Models;
using System.Collections.Generic;
using BillPayments.Application.Common.Models;
using BillPayments.Domain.Model.PurchaseToken;

namespace BillPayments.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ZesaController : ApiController
    {

        private readonly ILogger<ZesaController> _logger;
        private readonly IConfiguration _config;
        private readonly IBackgroundTaskService _backgroundTaskService;

        public ZesaController(ILogger<ZesaController> logger, IConfiguration config, IBackgroundTaskService backgroundTaskService)
        {
            _logger = logger;
            _config = config;
            _backgroundTaskService = backgroundTaskService;
        }

        [HttpGet("GetCustomerInfo")]
        public async Task<ActionResult<CustomerInfo>> GetCustomerInfo(string MeterNumber)
        {
            var customerInfoResponse = await Mediator.Send(new GetCustomerInfoCommand(
                new CustomerInfoRequest
                {
                    TransactionAmount = AmountHelper.BillPaymentAmount(10000),
                    UtilityAccount = MeterNumber,
                    VendorNumber = _config["ZESA_VendorNumber"],
                    MerchantName = Merchants.ZETDC,
                    ProductName = Products.ZETDC_PREPAID,
                    VendorReference = Guid.NewGuid().ToString(),
                    CurrencyCode = CurrencyCodes.ZWG,
                    APIVersion = _config["ZESA_API_Version"],
                }));

            var customerInfo = new CustomerInfo
            {
                Address = customerInfoResponse.CustomerAddress,
                CustomerName = customerInfoResponse.CustomerData,
                MeterNumber = customerInfoResponse.UtilityAccount,
                Reference = customerInfoResponse.TransactionReference,
                Currency =  customerInfoResponse.CurrencyCode,
            };

            return Ok(customerInfo);
        }

        [HttpGet("GetBalance")]
        public async Task<ActionResult<VendorBalance>> GetBalance()
        {
            var vendorNumber = _config["ZESA_VendorNumber"];
            var accountNumber = _config["ZESA_AccountNumber"];
            var vendorBalanceResponse = await Mediator.Send(
                new GetVendorBalanceCommand(new VendorBalanceRequest
                {
                    VendorNumber = vendorNumber,
                    AccountNumber = accountNumber,
                    CurrencyCode = CurrencyCodes.ZWG,
                    VendorReference = Guid.NewGuid().ToString(),
                    APIVersion = _config["ZESA_API_Version"]
                }));
            if (vendorBalanceResponse.ResponseCode != "00")
                return BadRequest(vendorBalanceResponse.Narrative);

            var balance = VendorBalanceHelper.GetBalanceAmount(vendorBalanceResponse.VendorBalance);
            var vendorBalance = new VendorBalance
            {
                Balance = balance.ToString(),
                AccountNumber = accountNumber,
                CurrencyCode = vendorBalanceResponse.CurrencyCode,
                Reference = vendorBalanceResponse.VendorReference
            };

            return Ok(vendorBalance);
        }

        [HttpGet("GetBalanceUSD")]
        public async Task<ActionResult<VendorBalance>> GetBalanceUSD()
        {
            var vendorNumber = _config["ZESA_VendorNumber"];
            var accountNumber = _config["ZESA_AccountNumber_USD"];
            var vendorBalanceResponse = await Mediator.Send(
                new GetVendorBalanceCommand(new VendorBalanceRequest
                {
                    VendorNumber = vendorNumber,
                    AccountNumber = accountNumber,
                    CurrencyCode = CurrencyCodes.USD,
                    VendorReference = Guid.NewGuid().ToString(),
                         APIVersion = _config["ZESA_API_Version"],
                }));
            if (vendorBalanceResponse.ResponseCode != "00")
                return BadRequest(vendorBalanceResponse.Narrative);

            var balance = VendorBalanceHelper.GetBalanceAmount(vendorBalanceResponse.VendorBalance);
            var vendorBalance = new VendorBalance
            {
                Balance = balance.ToString(),
                AccountNumber = accountNumber,
                CurrencyCode = vendorBalanceResponse.CurrencyCode,
                Reference = vendorBalanceResponse.VendorReference
            };

            return Ok(vendorBalance);
        }

        [HttpPost("PurchaseToken")]
        public async Task<ActionResult<PurchaseToken>> PurchaseToken(decimal transactionAmount, string utilityAccount, string reference)
        {
       
            var tokenRequest = new PurchaseTokenRequest
            {
                VendorReference = reference,
                TransactionAmount = AmountHelper.BillPaymentAmount(transactionAmount),
                VendorNumber = _config["ZESA_VendorNumber"],
                TerminalId = "POS001",
                MerchantName = Merchants.ZETDC,
                ProductName = Products.ZETDC_PREPAID,
                UtilityAccount = utilityAccount,
                Aggregator = "POWERTEL",
                CurrencyCode = CurrencyCodes.ZWG,
                APIVersion = _config["ZESA_API_Version"],
            };

            var response = await Mediator.Send(new PurchaseTokenCommand(tokenRequest));

            if (response.ResponseCode == "09")
            {
                var purchaseResponse = PurchaseTokenHelper.GetPurchaseToken(response);

                await _backgroundTaskService.SaveTask(new BackgroundTask
                {
                    EntityBody = JsonSerializer.Serialize<PurchaseTokenRequest>(tokenRequest),
                    EntityType = tokenRequest.GetType().ToString(),
                    DateCreated = DateTime.Now,
                    NumberOfRetries = 0,
                    RetrySucceeded = false
                });


                return Ok(purchaseResponse);
            }
            else
            {
                var purchaseResponse = PurchaseTokenHelper.GetPurchaseToken(response);
                return Ok(purchaseResponse);
            }
        }

        [HttpPost("PurchaseTokenUSD")]
        public async Task<ActionResult<PurchaseToken>> PurchaseTokenUSD(decimal transactionAmount, string utilityAccount, string reference)
        {
            var tokenRequest = new PurchaseTokenRequest
            {
                VendorReference = reference,
                TransactionAmount = AmountHelper.BillPaymentAmount(transactionAmount),
                VendorNumber = _config["ZESA_VendorNumber"],
                TerminalId = "POS001",
                MerchantName = Merchants.ZETDC,
                ProductName = Products.ZETDC_PREPAID,
                UtilityAccount = utilityAccount,
                Aggregator = "POWERTEL",
                CurrencyCode = CurrencyCodes.USD,
                APIVersion = _config["ZESA_API_Version"],
            };

            var response = await Mediator.Send(new PurchaseTokenCommand(tokenRequest));

            if (response.ResponseCode == "09")
            {
                var purchaseResponse = PurchaseTokenHelper.GetPurchaseToken(response);

                await _backgroundTaskService.SaveTask(new BackgroundTask
                {
                    EntityBody = JsonSerializer.Serialize<PurchaseTokenRequest>(tokenRequest),
                    EntityType = tokenRequest.GetType().ToString(),
                    DateCreated = DateTime.Now,
                    NumberOfRetries = 0,
                    RetrySucceeded = false
                });


                return Ok(purchaseResponse);
            }
            else
            {
                var purchaseResponse = PurchaseTokenHelper.GetPurchaseToken(response);
                return Ok(purchaseResponse);
            }
        }

        [HttpPost("QueryTokenPurchase")]
        public async Task<ActionResult<PurchaseToken>> QueryTokenPurchase(decimal transactionAmount, string originalReference, string utilityAccount)
        {
            var response = await Mediator.Send(new ResendPurchaseTokenCommand(
                new ResendPurchaseTokenRequest
                {
                    OriginalReference = originalReference,
                    VendorReference = Guid.NewGuid().ToString(),
                    TransactionAmount = AmountHelper.BillPaymentAmount(transactionAmount),
                    VendorNumber = _config["ZESA_VendorNumber"],
                    MerchantName = Merchants.ZETDC,
                    ProductName = Products.ZETDC_PREPAID,
                    UtilityAccount = utilityAccount,
                    CurrencyCode = CurrencyCodes.ZWG,
                    APIVersion = _config["ZESA_API_Version"],
                }));

            if (response.ResponseCode != "00")
            {
                var purchaseResponse = PurchaseTokenHelper.GetPurchaseToken(response);

                return Ok(purchaseResponse);
            }
            else
            {
                var purchaseResponse = PurchaseTokenHelper.GetPurchaseToken(response);
                return Ok(purchaseResponse);
            }
        }

        [HttpPost("QueryTokenPurchaseUSD")]
        public async Task<ActionResult<PurchaseToken>> QueryTokenPurchaseUSD(decimal transactionAmount, string originalReference, string utilityAccount)
        {
            var response = await Mediator.Send(new ResendPurchaseTokenCommand(
                new ResendPurchaseTokenRequest
                {
                    OriginalReference = originalReference,
                    VendorReference = Guid.NewGuid().ToString(),
                    TransactionAmount = AmountHelper.BillPaymentAmount(transactionAmount),
                    VendorNumber = _config["ZESA_VendorNumber"],
                    MerchantName = Merchants.ZETDC,
                    ProductName = Products.ZETDC_PREPAID,
                    UtilityAccount = utilityAccount,
                    CurrencyCode = CurrencyCodes.USD,
                    APIVersion = _config["ZESA_API_Version"],
                }));

            if (response.ResponseCode != "00")
            {
                var purchaseResponse = PurchaseTokenHelper.GetPurchaseToken(response);

                return Ok(purchaseResponse);
            }
            else
            {
                var purchaseResponse = PurchaseTokenHelper.GetPurchaseToken(response);
                return Ok(purchaseResponse);
            }
        }



    }
}
