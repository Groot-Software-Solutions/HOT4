using BillPayments.Application.Commands;
using BillPayments.Domain.Enums;
using BillPayments.Domain.Helpers;
using BillPayments.Domain.Models;
using BillPayments.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BillPayments.WebAPI.Controllers
{
    public class HarareCityController : ApiController
    {
        private readonly ILogger<HarareCityController> _logger;
        private readonly IConfiguration _config;

        public HarareCityController(ILogger<HarareCityController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpGet("GetCustomerInfo")]
        public async Task<ActionResult<CustomerInfo>> GetCustomerInfo(string MeterNumber)
        {
            var customerInfoResponse = await Mediator.Send(new GetCustomerInfoCommand(
                new CustomerInfoRequest
                {
                    TransactionAmount = AmountHelper.BillPaymentAmount(0),
                    UtilityAccount = MeterNumber,
                    VendorNumber = _config["ZESA_VendorNumber"],
                    MerchantName = Merchants.HARARE,
                    ProductName = Products.HARARE,
                    VendorReference = Guid.NewGuid().ToString()
                })); ;

            var customerInfo = new CustomerInfo
            {
                Address = customerInfoResponse.CustomerAddress,
                CustomerName = customerInfoResponse.CustomerData,
                MeterNumber = customerInfoResponse.UtilityAccount,
                Reference = customerInfoResponse.TransactionReference
            };

            return Ok(customerInfo);
        }

        [HttpPost("MakePayment")]
        public async Task<ActionResult<PaymentResponse>> MakePayment(decimal transactionAmount, string currencyCode, string utilityAccount)
        {
            var response = await Mediator.Send(new MakePaymentCommand(
                new PaymentRequest
                {
                    VendorReference = Guid.NewGuid().ToString(),
                    ProcessingCode = ProcessingCodes.TokenPurchaseRequest,
                    TransactionAmount = AmountHelper.BillPaymentAmount(transactionAmount),
                    VendorNumber = _config["ZESA_VendorNumber"],
                    MerchantName = Merchants.HARARE,
                    ProductName = Products.HARARE,
                    UtilityAccount = utilityAccount,
                    CurrencyCode = currencyCode,
                })); ;

            return Ok(response);
        }
    }
}
