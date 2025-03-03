using MediatR;
using OneOf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using TelOne.Application.Common.Exceptions;
using TelOne.Application.Common.Interfaces;
using TelOne.Application.Common.Models;
using TelOne.Domain.Models;

namespace TelOne.Application.Commands.BroadbandProducts
{
    public class QueryBroadbandProductsCommandUSD : IRequest<OneOf<List<BroadbandProduct>, APIException, string>>
    {
        public QueryBroadbandProductsRequest Item { get; set; }

        public QueryBroadbandProductsCommandUSD(QueryBroadbandProductsRequest item)
        {
            Item = item;
        }

        public class QueryBroadbandProductsCommandUSDHandler : IRequestHandler<QueryBroadbandProductsCommandUSD, OneOf<List<BroadbandProduct>, APIException, string>>
        {
            private readonly IAPIService _apiService;

            public QueryBroadbandProductsCommandUSDHandler(IAPIService apiService)
            {
                _apiService = apiService;
            }
            public async Task<OneOf<List<BroadbandProduct>, APIException, string>> Handle(QueryBroadbandProductsCommandUSD request, CancellationToken cancellationToken)
            {
                try
                {

                    return await _apiService
                        .Post<List<BroadbandProduct>, QueryBroadbandProductsRequest>
                        ("getBroadbandProducts/USD", request.Item);

                }
                catch (Exception ex)
                {
                    throw new APIException("Telone", ex.Message);
                }
            }
        }
    }
}
