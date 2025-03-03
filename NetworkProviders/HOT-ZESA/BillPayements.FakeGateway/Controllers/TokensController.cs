using BillPayments.Domain.Models;
using Bogus;
using Microsoft.AspNetCore.Mvc;

namespace BillPayements.FakeGateway.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class TokensController : Controller
    {
        [HttpPost("purchaseToken")]
        public PurchaseTokenResponse PurchaseToken([FromBody] PurchaseTokenRequest purchaseToken)
        {
            var responseCodes = new[] { "00", "09", "01" };

            var fakeResponse = new Faker<PurchaseTokenResponse>()
                .RuleFor(v => v.TransactionReference, f => f.Random.Replace("???#############"))
                .Rules((f, v) =>
                {
                    var responseCode = f.PickRandom(responseCodes);
                    if (responseCode == "00")
                    {
                        v.Narrative = "Success";
                        v.ResponseCode = responseCode;
                    }
                    else if (responseCode == "09")
                    {
                        v.Narrative = "Pending";
                        v.ResponseCode = responseCode;
                    }
                    else
                    {
                        v.Narrative = "Failed";
                        v.ResponseCode = responseCode;
                    }
                })
                .RuleFor(v => v.Arrears, f => f.Random.Replace("Debt Recovery|#########|####|#|####"))
                .RuleFor(v => v.Token, f =>
                    f.Random.Replace("####################|##.#|##.# kWh @ 2.0 $/KWh:::|POWERT3EMDB#######|####|####|0%"))
                .RuleFor(v => v.FixedCharges, f => f.Random.Replace("RE Levy (6%)|POWERT3EMDB#######|###|#|6%"));

            var response = fakeResponse.Generate();
            response.Mti = purchaseToken.Mti;
            response.VendorReference = purchaseToken.VendorReference;
            response.ProcessingCode = purchaseToken.ProcessingCode;
            response.TransactionAmount = purchaseToken.TransactionAmount.ToString();
            response.TransmissionDate = purchaseToken.TransmissionDate;
            response.VendorNumber = purchaseToken.VendorNumber;
            response.UtilityAccount = purchaseToken.UtilityAccount;
            response.PaymentType = "PREPAID";
            response.MiscellaneousData = string.Empty;
            response.CurrencyCode = "ZWL";
            response.MerchantName = purchaseToken.MerchantName;
            response.ProductName = purchaseToken.ProductName;

            return response;
        }

        [HttpPost("resendpuchaseToken")]
        public PurchaseTokenResponse ResendpuchaseToken([FromBody] PurchaseTokenRequest purchaseToken)
        {
            var responseCodes = new[] { "00", "09", "01" };

            var fakeResponse = new Faker<PurchaseTokenResponse>()
                .RuleFor(v => v.TransactionReference, f => f.Random.Replace("???#############"))
                .Rules((f, v) =>
                {
                    var responseCode = f.PickRandom(responseCodes);
                    if (responseCode == "00")
                    {
                        v.Narrative = "Success";
                        v.ResponseCode = responseCode;
                    }
                    else if (responseCode == "09")
                    {
                        v.Narrative = "Pending";
                        v.ResponseCode = responseCode;
                    }
                    else
                    {
                        v.Narrative = "Failed";
                        v.ResponseCode = responseCode;
                    }
                })
                .RuleFor(v => v.Arrears, f => f.Random.Replace("Debt Recovery|#########|####|#|####"))
                .RuleFor(v => v.Token, f =>
                    f.Random.Replace("####################|##.#|##.# kWh @ 2.0 $/KWh:::|POWERT3EMDB#######|####|####|0%"))
                .RuleFor(v => v.FixedCharges, f => f.Random.Replace("RE Levy (6%)|POWERT3EMDB#######|###|#|6%"));

            var response = fakeResponse.Generate();
            response.Mti = purchaseToken.Mti;
            response.VendorReference = purchaseToken.VendorReference;
            response.ProcessingCode = purchaseToken.ProcessingCode;
            response.TransactionAmount = purchaseToken.TransactionAmount.ToString();
            response.TransmissionDate = purchaseToken.TransmissionDate;
            response.VendorNumber = purchaseToken.VendorNumber;
            response.UtilityAccount = purchaseToken.UtilityAccount;
            response.PaymentType = "PREPAID";
            response.MiscellaneousData = string.Empty;
            response.CurrencyCode = "ZWL";
            response.MerchantName = purchaseToken.MerchantName;
            response.ProductName = purchaseToken.ProductName;

            return response;
        }
    }
}
