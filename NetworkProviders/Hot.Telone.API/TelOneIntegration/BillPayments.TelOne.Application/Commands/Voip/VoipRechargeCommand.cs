using MediatR;
using Microsoft.Extensions.Configuration;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TelOne.Application.Common.Exceptions;
using TelOne.Application.Common.Interfaces;
using TelOne.Application.Common.Models;
using TelOne.Domain.Models;

namespace TelOne.Application.Commands.Voip
{
    public class VoipRechargeCommand : IRequest<OneOf<VoipRechargeResponse, APIException, string>>
    {
        public VoipRechargeCommand(VoipRechargeRequest item)
        {
            Item = item;
        }

        public VoipRechargeRequest Item { get; set; }

        public class VoipRechargeCommandHandler : IRequestHandler<VoipRechargeCommand, OneOf<VoipRechargeResponse, APIException, string>>
        {
            private readonly IAPIService apiService;
            private readonly IConfiguration configuration;

            public VoipRechargeCommandHandler(IAPIService apiService, IConfiguration configuration)
            {
                this.apiService = apiService;
                this.configuration = configuration;
            }

            public async Task<OneOf<VoipRechargeResponse, APIException, string>> Handle(VoipRechargeCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await apiService
                        .Post<VoipRechargeResponse, VoipRechargeRequest>
                        ("cashAccountRecharge", request.Item);
                    if (result.IsT0)
                    {
                        try
                        {
                            var item = result.AsT0;
                            var requestBal = new Request()
                            {
                                APIKey = configuration["APIKey"],
                                AccountSid = configuration["AccountSid"],
                            };
                            var balanceResponse = await apiService.Post<List<AccountBalance>, Request>("QueryMerchantBalance", requestBal);
                            if (balanceResponse.IsT0)
                            {
                                item.Balance = balanceResponse.AsT0.Where(b => b.Currency.ToUpper().Contains("ZiG")).FirstOrDefault().Balance;
                                return item;
                            }
                        }
                        catch (Exception) { }
                    }
                    return result;

                }
                catch (Exception ex)
                {
                    throw new APIException("Telone", ex.Message);
                }
            }
        }
    }
}
