using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Infrastructure.Extensions;

namespace Infrastructure.Services
{
    public class RechargeService : IRechargeHelper
    {
        //private readonly HotAPIClient hotAPIClient;
        readonly IAPIHelper apiservice;
        public RechargeService(IAPIHelper apiservice)
        {
            // hotAPIClient = new HotAPIClient(options.Value.BaseUrl, options.Value.AccessCode, options.Value.AccessPassword);
            this.apiservice = apiservice; 
            apiservice.APIName = "HOT";
        }

        public async Task<bool> SubmitSelfTopUp(SelfTopUp request)
        { 
            var response = await apiservice.APIPostCall<APIResponse, SelfTopUp>("dealers/selftopup", request, Guid.NewGuid().ToString());
            return true; //response.ReplyCode == 2;

        }
    }
}
