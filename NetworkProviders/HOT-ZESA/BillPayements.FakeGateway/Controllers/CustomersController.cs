using BillPayments.Domain.Models;
using Bogus;
using Microsoft.AspNetCore.Mvc;

namespace BillPayements.FakeGateway.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        [HttpPost("getCustomerInfo")]
        public CustomerInfoResponse GetCustomerInfo([FromBody] CustomerInfoRequest customerInfo)
        {
            var responseCodes = new[] { "00", "09", "01" };

            var fakeResponse = new Faker<CustomerInfoResponse>()
                .RuleFor(v => v.TransactionReference, f => f.Random.Replace("???#############"))
                .RuleFor(v => v.CustomerData, f => f.Person.FullName)
                .RuleFor(v => v.CustomerAddress, f => f.Address.StreetAddress());

            var response = fakeResponse.Generate();
            response.Mti = "0210";
            response.VendorReference = customerInfo.VendorReference;
            response.ProcessingCode = customerInfo.ProcessingCode;
            response.TransmissionDate = customerInfo.TransmissionDate;
            response.ResponseCode = "00";
            response.VendorNumber = customerInfo.VendorNumber;
            response.TransactionAmount = customerInfo.TransactionAmount.ToString();
            response.MerchantName = customerInfo.MerchantName;
            response.UtilityAccount = customerInfo.UtilityAccount;
            response.ProductName = customerInfo.ProductName;

            return response;
        }
    }
}
