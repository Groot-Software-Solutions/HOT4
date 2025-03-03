using TelOne.Application.Common.Exceptions;
using TelOne.Application.Common.Interfaces;
using TelOne.Application.Common.Models; 
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TelOne.Domain.Models;
using OneOf;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace TelOne.Application.BroadbandProducts
{
    public class QueryBroadbandProductsCommand : IRequest<OneOf<List<BroadbandProduct>, APIException, string>>
    {
        public QueryBroadbandProductsRequest Item { get; set; }

        public QueryBroadbandProductsCommand(QueryBroadbandProductsRequest item)
        {
            Item = item;
        }

        public class QueryBroadbandProductsCommandHandler : IRequestHandler<QueryBroadbandProductsCommand, OneOf<List<BroadbandProduct>, APIException, string>>
        {
            private readonly IAPIService _apiService; 

            public QueryBroadbandProductsCommandHandler(IAPIService apiService)
            {
                _apiService = apiService; 
            }

            public async Task<OneOf<List<BroadbandProduct>,APIException,string>> Handle(QueryBroadbandProductsCommand request, CancellationToken cancellationToken)
            { 
                try
                {
                    return  await _apiService
                        .Post<List<BroadbandProduct>, QueryBroadbandProductsRequest>
                        ("getBroadbandProducts", request.Item);
                     
                }
                catch (Exception ex)
                {
                    throw new APIException("Telone", ex.Message);
                } 
            }
        }
    }
}
