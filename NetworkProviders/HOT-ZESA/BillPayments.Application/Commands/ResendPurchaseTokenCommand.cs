using BillPayments.Application.Common.Exceptions;
using BillPayments.Application.Common.Interfaces;
using BillPayments.Domain.Models.PurchaseToken;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BillPayments.Application.Commands
{
    public class ResendPurchaseTokenCommand : IRequest<ResendPurchaseTokenResponse>
    {
        public ResendPurchaseTokenRequest Item { get; set; }

        public ResendPurchaseTokenCommand(ResendPurchaseTokenRequest item)
        {
            Item = item;
        }

        public class ResendPurchaseTokenCommandHandler : IRequestHandler<ResendPurchaseTokenCommand, ResendPurchaseTokenResponse>
        {
            private readonly IAPIService apihelper;

            public ResendPurchaseTokenCommandHandler(IAPIService apihelper)
            {
                this.apihelper = apihelper;
            }

            public async Task<ResendPurchaseTokenResponse> Handle(ResendPurchaseTokenCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    return await apihelper.APIPostCall<ResendPurchaseTokenResponse, ResendPurchaseTokenRequest>("vend", request.Item);
                }
                catch (Exception ex)
                {
                    throw new APIException(request.Item.MerchantName, ex.Message);
                }
            }
        }
    }
}
