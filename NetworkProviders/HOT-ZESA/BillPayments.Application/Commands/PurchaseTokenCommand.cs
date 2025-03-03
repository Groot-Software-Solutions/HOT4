using BillPayments.Application.Common.Exceptions;
using BillPayments.Application.Common.Interfaces;
using BillPayments.Application.Common.Models;
using BillPayments.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BillPayments.Application.Zetdc.Tokens
{
    public class PurchaseTokenCommand : IRequest<PurchaseTokenResponse>
    {
        public PurchaseTokenRequest Item { get; set; }

        public PurchaseTokenCommand(PurchaseTokenRequest item)
        {
            Item = item;
        }

        public class PurchaseTokenCommandHanler : IRequestHandler<PurchaseTokenCommand, PurchaseTokenResponse>
        {
            readonly IAPIService apihelper;
            readonly ILogger logger;

            public PurchaseTokenCommandHanler(IAPIService apihelper, ILogger logger)
            {
                this.apihelper = apihelper;
                this.logger = logger;
            }

            public async Task<PurchaseTokenResponse> Handle(PurchaseTokenCommand request, CancellationToken cancellationToken)
            {
                var result = new PurchaseTokenResponse();
                try
                {
                    result = await apihelper.APIPostCall<PurchaseTokenResponse, PurchaseTokenRequest>("vend", request.Item); 
                }
                catch (Exception ex)
                {
                    result.Narrative = ex.Message;
                    throw new APIException(request.Item.MerchantName, ex.Message);
                }
                finally
                {
                    try
                    {
 logger.Save(new ZesaLogItem(request.Item,result));
                    }
                    catch (Exception)
                    { 
                    } 
                } 
                return result;
            }
        }

    }


}
