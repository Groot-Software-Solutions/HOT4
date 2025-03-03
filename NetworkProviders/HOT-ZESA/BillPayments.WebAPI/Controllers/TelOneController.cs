using BillPayments.Application.Commands;
using BillPayments.Application.Services;
using BillPayments.Domain.Enums;
using BillPayments.Domain.Helpers;
using BillPayments.Domain.Models;
using BillPayments.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillPayments.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TelOneController : ApiController
    {
        private readonly ILogger<TelOneController> _logger;
        private readonly IConfiguration _config;
        private readonly IBackgroundTaskService _backgroundTaskService;

        public TelOneController(ILogger<TelOneController> logger, IConfiguration config, IBackgroundTaskService backgroundTaskService)
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
                    UtilityAccount = MeterNumber,
                    VendorNumber = _config["ZESA_VendorNumber"],
                    MerchantName = Merchants.TELONE,
                    ProductName = Products.ZETDC_PREPAID,
                    VendorReference = Guid.NewGuid().ToString()
                }));

            var customerInfo = new CustomerInfo
            {
                CustomerName = customerInfoResponse.CustomerData,
                MeterNumber = customerInfoResponse.UtilityAccount,
                Reference = customerInfoResponse.TransactionReference
            };

            return Ok(customerInfo);
        }

        [HttpPost("DirectPayment")]
        public async Task<ActionResult<PaymentResponse>> DirectPayment(decimal transactionAmount, string currencyCode, string utilityAccount, string sourceMobile)
        {
            var response = await Mediator.Send(new MakePaymentCommand(
                new PaymentRequest
                {
                    VendorReference = Guid.NewGuid().ToString(),
                    TransactionAmount = AmountHelper.BillPaymentAmount(transactionAmount),
                    VendorNumber = _config["ZESA_VendorNumber"],
                    SourceMobile = sourceMobile,
                    MerchantName = Merchants.TELONE,
                    ProductName = Products.TELONE,
                    UtilityAccount = utilityAccount,
                    CurrencyCode = currencyCode,
                    RequiresVoucher = "false"
                }));

            return Ok(response);
        }

        [HttpPost("VoucherPayment")]
        public async Task<ActionResult<PaymentResponse>> VoucherPayment(decimal transactionAmount, string currencyCode, string utilityAccount, string sourceMobile)
        {
            var response = await Mediator.Send(new MakePaymentCommand(
                new PaymentRequest
                {
                    VendorReference = Guid.NewGuid().ToString(),
                    TransactionAmount = AmountHelper.BillPaymentAmount(transactionAmount),
                    VendorNumber = _config["ZESA_VendorNumber"],
                    SourceMobile = sourceMobile,
                    MerchantName = Merchants.ZOL,
                    ProductName = Products.ZOL_DATA,
                    UtilityAccount = utilityAccount,
                    CurrencyCode = currencyCode,
                    RequiresVoucher = "true"
                }));

            return Ok(response);
        }
    }
}
