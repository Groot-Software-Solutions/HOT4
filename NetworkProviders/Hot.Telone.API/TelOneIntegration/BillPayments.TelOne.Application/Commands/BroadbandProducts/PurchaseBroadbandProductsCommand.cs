using TelOne.Application.Common.Exceptions;
using TelOne.Application.Common.Interfaces;
using TelOne.Application.Common.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using OneOf;
using System.Collections.Generic;
using TelOne.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace TelOne.Application.BroadbandProducts
{
    public class PurchaseBroadbandProductsCommand : IRequest<OneOf<PurchaseBroadbandProductsResponse, APIException, string>>
    {
        public PurchaseBroadbandProductsRequest Item { get; set; }

        public PurchaseBroadbandProductsCommand(PurchaseBroadbandProductsRequest item)
        {
            Item = item;
        }

        public class PurchaseBroadbandProductsCommandHandler : IRequestHandler<PurchaseBroadbandProductsCommand, OneOf<PurchaseBroadbandProductsResponse, APIException, string>>
        {
            private readonly IAPIService _apiService;
            private readonly IConfiguration _configuration;

            public PurchaseBroadbandProductsCommandHandler(IAPIService apiService)
            {
                _apiService = apiService;
            }

            public PurchaseBroadbandProductsCommandHandler(IAPIService apiService, IConfiguration configuration)
            {
                _apiService = apiService;
                _configuration = configuration;
            }

            public async Task<OneOf<PurchaseBroadbandProductsResponse, APIException, string>> Handle(PurchaseBroadbandProductsCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await _apiService.Post<PurchaseBroadbandProductsResponse, PurchaseBroadbandProductsRequest>("purchaseBroadband", request.Item);
                    if (result.IsT0)
                    {

                        try
                        {
                            var item = result.AsT0;
                            var requestBal = new Request()
                            {
                                APIKey = _configuration["APIKey"],
                                AccountSid = _configuration["AccountSid"],
                            };
                            var balanceResponse = await _apiService.Post<List<AccountBalance>, Request>("QueryMerchantBalance", requestBal);
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
