using Hot.Nyaradzo.API.Models;
using Hot.Nyaradzo.Application.Common.Interfaces;
using Hot.Nyaradzo.Application.Common.Models;
using Hot.Nyaradzo.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hot.Nyaradzo.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NyaradzoController : ControllerBase
    {

        private readonly INyaradzoServiceFactory nyaradzo;

        public NyaradzoController(INyaradzoServiceFactory nyaradzo)
        {
            this.nyaradzo = nyaradzo;
        }

        [HttpGet("AccountEnquiry")]
        public ActionResult<NyaradzoResultModel> AccountEnquiry(string policyNumber, string Reference)
        {
            var service = nyaradzo.GetService();
            return Ok(service.AccountEnquiry(policyNumber, Reference));
        }

        [HttpPost("PaymentProcessing")]
        public ActionResult<NyaradzoResultModel> ProcessPayment([FromBody] PaymentProcessingRequest request)
        {
            var service = nyaradzo.GetService();
            return Ok(service.ProcessPayment(
                request.PolicyNumber,
                request.Reference,
                request.AmountPaid,
                request.MonthlyPremium,
                request.NumberOfMonthsPaid,
                request.Date,
               request.currency));
        }

        [HttpPost("Reversal")]
        public ActionResult<NyaradzoResultModel> TransactionReversal([FromBody] ReversalRequest request)
        {
            var service = nyaradzo.GetService();
            return Ok(service.TransactionReversal(request.Reference));
        }

    }

   
}
