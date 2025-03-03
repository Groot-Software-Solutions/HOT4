using TelOne.Application.Common.Exceptions;
using TelOne.Application.Common.Interfaces;
using TelOne.Application.Common.Models; 
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using OneOf;
using System.Collections.Generic;
using TelOne.Domain.Models;
using System.Linq;

namespace TelOne.Application.AdslAccount
{
    public class RechargeAdslAccountCommand : IRequest<OneOf<RechargeAdslAccountResponse, APIException, string>>
    {
        public RechargeAdslAccountRequest Item { get; set; }
        public RechargeAdslAccountCommand(RechargeAdslAccountRequest item)
        {
            Item = item;
        }

        public class RechargeAdslAccountCommandHandler : IRequestHandler<RechargeAdslAccountCommand, OneOf<RechargeAdslAccountResponse, APIException, string>>
        {
            private readonly IAPIService apiService;
            private readonly IConfiguration configuration;

            public RechargeAdslAccountCommandHandler(IAPIService apiService)
            {
                this.apiService = apiService;
            }
            public RechargeAdslAccountCommandHandler(IAPIService apiService, IConfiguration configuration)
            {
                this.apiService = apiService;
                this.configuration = configuration;
            }

            public async Task<OneOf<RechargeAdslAccountResponse,APIException,string>> Handle(RechargeAdslAccountCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var result  =  await apiService
                        .Post<RechargeAdslAccountResponse, RechargeAdslAccountRequest>
                        ("rechargeBroadband", request.Item);

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
