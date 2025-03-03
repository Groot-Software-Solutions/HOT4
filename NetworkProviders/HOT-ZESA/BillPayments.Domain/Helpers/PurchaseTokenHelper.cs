using BillPayments.Domain.Model.PurchaseToken;
using BillPayments.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BillPayments.Domain.Helpers
{
    public static class PurchaseTokenHelper
    {

        public static PurchaseToken GetPurchaseToken(PurchaseTokenResponse response)
        {
            var result = new PurchaseToken
            {
                Date = DateHelper.ParseBillPaymentFormat(response.TransmissionDate),
                Amount = AmountHelper.ActualAmount(response.TransactionAmount),
                RawResponse = JsonSerializer.Serialize(response),
                MeterNumber = response.UtilityAccount,
                ResponseCode = response.ResponseCode,
                Narrative = response.Narrative,
                Reference = response.TransactionReference,
                VendorReference = response.VendorReference
            };

            result.Tokens = GetTokens(response);


            return result;
        }

        public static List<TokenItem> GetTokens(PurchaseTokenResponse response)
        {
            var result = new List<TokenItem>();
            if (TokenHelper.IsValidInput(response.Token))
                result.Add(GetToken(response.Arrears, response.FixedCharges, response.Token));
            if (TokenHelper.IsDoubleToken(response.Token))
            {
                for (int i = 1; i < response.Token.Split("#").Length; i++)
                {
                    result.Add(GetToken("", "", response.Token.Split("#")[i]));
                } 
            }
                
            return result;
        }

        public static TokenItem GetToken(string Arreas, string FixedCharges, string Token)
        {
            return new TokenItem()
            {
                Arrears = TokenHelper.GetArrears(Arreas),
                Levy = TokenHelper.GetLevy(FixedCharges),
                Token = TokenHelper.GetFormattedToken(Token),
                Units = TokenHelper.GetUnits(Token),
                TaxAmount = TokenHelper.GetTaxAmount(Token),
                NetAmount = TokenHelper.GetNetAmount(Token),
                ZESAReference = TokenHelper.GetZESAReference(Token),
            };
        }

    }
}
