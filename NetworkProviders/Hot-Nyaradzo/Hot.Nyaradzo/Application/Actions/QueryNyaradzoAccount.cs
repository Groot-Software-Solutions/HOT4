using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Extensions;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models.RechargeServiceModels.Nyaradzo;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace Hot.Nyaradzo.Application.Actions
{
    public record QueryNyaradzoAccount(string PolicyNumber) : IRequest<OneOf<NyaradzoResult, AppException>>;
    public class QueryNyaradzoAccountHandler : IRequestHandler<QueryNyaradzoAccount, OneOf<NyaradzoResult, AppException>>
    {
        private readonly INyaradzoRechargeAPIService service;
        private readonly ILogger<QueryNyaradzoAccountHandler> logger;

        public QueryNyaradzoAccountHandler(INyaradzoRechargeAPIService service, ILogger<QueryNyaradzoAccountHandler> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        public async Task<OneOf<NyaradzoResult, AppException>> Handle(QueryNyaradzoAccount request, CancellationToken cancellationToken)
        {
            var response = await service.QueryAccount(request.PolicyNumber);
            if (response == null) return new AppException("WebAPI Exception -  No Response", "").LogAndReturnError(logger);
           return response;
        }
    }
}
