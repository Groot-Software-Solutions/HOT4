using Domain.Entities;
using Domain.Interfaces; 
using Infrastructure.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{ 
    public class DealerService : IDealerService
    {
        // private readonly HotAPIClient hotAPIClient;
        readonly IAPIHelper apiservice;
        public DealerService(  IAPIHelper apiservice)
        {
             this.apiservice= apiservice;
            apiservice.APIName = "HOT";
        }

        public async Task<Response> ResetPin(PinReset pinReset)
        {
            var response = await apiservice.APIPostCall<APIResponse, PinReset>  ("dealers/resetpin", pinReset, Guid.NewGuid().ToString());
            //var response = await apiservice.ResetPinAsync(pinReset.TargetMobile, pinReset.Names, pinReset.IDNumber, pinReset.Sender);

            return new Response() { 
                Message = response.ReplyMsg, 
                Status = (response.ReplyCode==2 ? Domain.Enum.State.Success: Domain.Enum.State.Failed) 
            };
        }
    }
}
