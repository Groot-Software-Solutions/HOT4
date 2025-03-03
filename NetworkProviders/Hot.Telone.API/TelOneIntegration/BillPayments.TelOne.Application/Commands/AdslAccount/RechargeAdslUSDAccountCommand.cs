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

namespace TelOne.Application.Commands.AdslAccount
{
    public class RechargeAdslUSDAccountCommand : IRequest<OneOf<RechargeAdslAccountResponse, APIException, string>>
    {
        public RechargeAdslAccountRequest Item { get; set; }
        public RechargeAdslUSDAccountCommand(RechargeAdslAccountRequest item)
        {
            Item = item;
        }

        public class RechargeAdslUSDAccountCommandHandler : IRequestHandler<RechargeAdslUSDAccountCommand, OneOf<RechargeAdslAccountResponse, APIException, string>>
        {
            private readonly IAPIService _apiService;

            public RechargeAdslUSDAccountCommandHandler(IAPIService apiService)
            {
                _apiService = apiService;
            }
            public async Task<OneOf<RechargeAdslAccountResponse, APIException, string>> Handle(RechargeAdslUSDAccountCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await _apiService
                       .Post<RechargeAdslAccountResponse, RechargeAdslAccountRequest>
                       ("rechargeBroadband", request.Item);


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
