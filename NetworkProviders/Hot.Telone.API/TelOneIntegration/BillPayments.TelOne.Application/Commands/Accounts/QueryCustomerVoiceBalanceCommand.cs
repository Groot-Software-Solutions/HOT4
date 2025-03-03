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

namespace TelOne.Application.Commands.Accounts
{
    public class QueryCustomerVoiceBalanceCommand : IRequest<OneOf<QueryCustomerVoiceBalanceResponse, APIException, string>>
    {
        public QueryCustomerVoiceBalanceCommand(QueryCustomerVoiceBalanceRequest item)
        {
            Item = item;
        }

        public QueryCustomerVoiceBalanceRequest Item { get; set; }

        public class QueryCustomerVoiceBalanceCommandHandler : IRequestHandler<QueryCustomerVoiceBalanceCommand, OneOf<QueryCustomerVoiceBalanceResponse, APIException, string>>
        {
            private readonly IAPIService _apiService;

            public QueryCustomerVoiceBalanceCommandHandler(IAPIService apiService)
            {
                _apiService = apiService;
            }

            public async Task<OneOf<QueryCustomerVoiceBalanceResponse, APIException, string>> Handle(QueryCustomerVoiceBalanceCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    return await _apiService
                        .Post<QueryCustomerVoiceBalanceResponse, QueryCustomerVoiceBalanceRequest>
                        ("QuerySubscriberVoiceBalance", request.Item);

                }
                catch (Exception ex)
                {
                    throw new APIException("Telone", ex.Message);
                }
            }
        }
    }
}
