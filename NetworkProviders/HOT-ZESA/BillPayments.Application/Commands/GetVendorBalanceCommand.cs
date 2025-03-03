using BillPayments.Application.Common.Exceptions;
using BillPayments.Application.Common.Interfaces;
using BillPayments.Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BillPayments.Application.Commands
{
    public class GetVendorBalanceCommand : IRequest<VendorBalanceResponse>
    {
        public VendorBalanceRequest Item { get; set; }
        public GetVendorBalanceCommand(VendorBalanceRequest item)
        {            
            Item = item;
        }       

        public class GetVendorBalanceCommandHandler : IRequestHandler<GetVendorBalanceCommand, VendorBalanceResponse>
        {
            readonly IAPIService apiservice;

            public GetVendorBalanceCommandHandler(IAPIService apihelper)
            {
                apiservice = apihelper;
            }

            public async Task<VendorBalanceResponse> Handle(GetVendorBalanceCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    return await apiservice.APIPostCall<VendorBalanceResponse, VendorBalanceRequest>("vend", request.Item);
                }
                catch (Exception ex)
                {
                    throw new APIException("ZETDC", ex.Message);
                }

            }
        }
    }
}
