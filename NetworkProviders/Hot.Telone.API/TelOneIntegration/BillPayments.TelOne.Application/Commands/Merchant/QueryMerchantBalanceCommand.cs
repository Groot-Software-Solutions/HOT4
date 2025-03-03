using MediatR;
using OneOf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TelOne.Application.Common.Exceptions;
using TelOne.Application.Common.Interfaces;
using TelOne.Application.Common.Models;
using TelOne.Domain.Models;

namespace TelOne.Application.Commands.Merchant
{

    public class QueryMerchantBalanceCommand : IRequest<OneOf<List<AccountBalance>, APIException, string>> 
    {
        public QueryMerchantBalanceCommand(Request item)
        {
            Item = item;
        }

        public Request Item { get; set; }

        public class QueryMerchantBalanceCommandHandler : IRequestHandler<QueryMerchantBalanceCommand, OneOf<List<AccountBalance>, APIException, string>>
        {
            private readonly IAPIService _apiService;

            public QueryMerchantBalanceCommandHandler(IAPIService apiService)
            {
                _apiService = apiService;
            }

            public async Task<OneOf<List<AccountBalance>, APIException, string>> Handle(QueryMerchantBalanceCommand request, CancellationToken cancellationToken)
            {
                try
                { 
                    return await _apiService
                        .Post<List<AccountBalance>,Request>
                        ("QueryMerchantBalance",request.Item); 
                }
                catch (Exception ex)
                {
                    throw new APIException("Telone", ex.Message);
                }
            }
        }
    }
}
