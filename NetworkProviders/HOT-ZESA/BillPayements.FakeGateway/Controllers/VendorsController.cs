using BillPayments.Domain.Models;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BillPayements.FakeGateway.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class VendorsController : Controller
    {
        [HttpPost("getVendorBalance")]
        public VendorBalanceResponse GetVendorBalance([FromBody]VendorBalanceRequest vendor)
        {
            var fakeResponse = new Faker<VendorBalanceResponse>()
                .RuleFor(v => v.TransactionReference, f => f.Random.Replace("???#############"));
            var response = fakeResponse.Generate();

            response.Mti = "0210";
            response.VendorReference = vendor.VendorReference;
            response.ProcessingCode = "300000";
            response.TransmissionDate = DateTime.Now.ToString("MMddyyyyHHmmss");
            response.VendorNumber = vendor.VendorNumber;
            response.TransactionReference = "Transaction Reference";
            response.AccountNumber = vendor.AccountNumber;
            response.CurrencyCode = vendor.CurrencyCode;
            response.ResponseCode = "00";
            response.Narrative = "Success";

            return response;
        }
    }
}
