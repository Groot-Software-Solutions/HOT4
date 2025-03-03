using MediatR;
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

namespace TelOne.Application.Commands.Merchant
{
    public class QueryMerchantTransactionsLedgerCommand: IRequest<OneOf<List<MerchantLedgerTransaction>, APIException, string>>
    {
        public QueryMerchantTransactionsLedgerCommand(QueryMerchantTransactionsRequest item)
        {
            Item = item;
        }

        public QueryMerchantTransactionsRequest  Item { get; set; }
    }
    public class QueryMerchantTransactionsLedgerCommandHandler : IRequestHandler<QueryMerchantTransactionsLedgerCommand, OneOf<List<MerchantLedgerTransaction>, APIException, string>>
    {
        private readonly IAPIService _apiService;

        public QueryMerchantTransactionsLedgerCommandHandler(IAPIService apiService)
        {
            _apiService = apiService;
        }

        public async Task<OneOf<List<MerchantLedgerTransaction>, APIException, string>> Handle(QueryMerchantTransactionsLedgerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _apiService
                    .Post<List<MerchantLedgerTransaction>, QueryMerchantTransactionsRequest>
                    ("QueryMerchantGL", request.Item);
            }
            catch (Exception ex)
            {
                throw new APIException("Telone", ex.Message);
            }
        }
    }
}
