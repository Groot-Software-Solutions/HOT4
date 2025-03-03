using MediatR;
using Microsoft.Extensions.Configuration;
using OneOf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using TelOne.Application.Common.Exceptions;
using TelOne.Application.Common.Interfaces;
using TelOne.Application.Common.Models;
using TelOne.Domain.Models;
using System.Linq;

namespace TelOne.Application.Commands.BroadbandProducts
{
    public class PurchaseBroadbandProductsUSDCommand : IRequest<OneOf<PurchaseBroadbandProductsResponse, APIException, string>>
    {
        public PurchaseBroadbandProductsRequest Item { get; set; }

        public PurchaseBroadbandProductsUSDCommand(PurchaseBroadbandProductsRequest item)
        {
            Item = item;
        }

        public class PurchaseBroadbandProductsUSDCommandHandler : IRequestHandler<PurchaseBroadbandProductsUSDCommand, OneOf<PurchaseBroadbandProductsResponse, APIException, string>>
        {
            private readonly IAPIService _apiService;
            private readonly IConfiguration _configuration;

            public PurchaseBroadbandProductsUSDCommandHandler(IAPIService apiService, IConfiguration configuration)
            {
                _apiService = apiService;
                _configuration = configuration;
            }

            public async Task<OneOf<PurchaseBroadbandProductsResponse, APIException, string>> Handle(PurchaseBroadbandProductsUSDCommand request, CancellationToken cancellationToken)
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
