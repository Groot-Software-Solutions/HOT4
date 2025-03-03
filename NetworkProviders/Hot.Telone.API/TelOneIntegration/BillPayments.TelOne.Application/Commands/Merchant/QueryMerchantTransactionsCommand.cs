using MediatR;
using OneOf;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TelOne.Application.Common.Exceptions;
using TelOne.Application.Common.Interfaces;
using TelOne.Application.Common.Models; 
using TelOne.Domain.Models;

namespace TelOne.Application.Commands.Merchant
{
    public class QueryMerchantTransactionsCommand : IRequest<OneOf<List<MerchantTransaction>, APIException, string>>
    {
        public QueryMerchantTransactionsCommand(QueryMerchantTransactionsRequest item)
        {
            Item = item;
        }

        public QueryMerchantTransactionsRequest Item { get; set; }

    }

    public class QueryMerchantTransactionsCommandHandler : IRequestHandler<QueryMerchantTransactionsCommand, OneOf<List<MerchantTransaction>, APIException, string>>
    {
        private readonly IAPIService _apiService;

        public QueryMerchantTransactionsCommandHandler(IAPIService apiService)
        {
            _apiService = apiService;
        }

        public async Task<OneOf<List<MerchantTransaction>, APIException, string>> Handle(QueryMerchantTransactionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _apiService
                    .Post<List<MerchantTransaction>, QueryMerchantTransactionsRequest>
                    ("QueryMerchantTransactions", request.Item);
            }
            catch (Exception ex)
            {
                throw new APIException("Telone", ex.Message);
            }
        }
    }
}
