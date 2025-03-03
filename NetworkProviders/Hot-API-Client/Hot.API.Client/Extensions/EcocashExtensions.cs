using Hot.API.Client.Common;
using Hot.API.Client.Models;
using Hot.API.Client.Ecocash.Domain.Entities;
using Hot.API.Client.Ecocash.Domain.Enums;
using Newtonsoft.Json.Linq;
using OneOf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Client.Extensions
{
    public static class EcocashExtensions
    {
        public static OneOf<Transaction, APIException, string> EcocashFundAccount(this HotAPIClient hotAPIClient, EcocashFundingRequest request)
        {
            return EcocashFundAccountAsync(hotAPIClient, request).Result;
        }

        public static async Task<OneOf<Transaction, APIException, string>> EcocashFundAccountAsync(this HotAPIClient hotAPIClient, EcocashFundingRequest request)
        {
            return await hotAPIClient.apiService
                .Post<Transaction, EcocashFundingRequest>("agents/ecocash-request", request, Guid.NewGuid().ToString());
        }
    }

   
}


