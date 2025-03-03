using BillPayments.Application.Common.Exceptions;
using BillPayments.Application.Common.Interfaces;

using BillPayments.Domain.Enums;
using BillPayments.Domain.Helpers;
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

namespace BillPayments.Application.Commands
{
    public class GetCustomerInfoCommand : IRequest<CustomerInfoResponse>
    {
        public CustomerInfoRequest Item { get; set; }

        public GetCustomerInfoCommand(CustomerInfoRequest item)
        {
            Item = item;
        }

        public class GetCustomerInfoCommandHandler : IRequestHandler<GetCustomerInfoCommand, CustomerInfoResponse>
        {
            readonly IAPIService apihelper;

            public GetCustomerInfoCommandHandler(IAPIService apihelper)
            {
                this.apihelper = apihelper;
            }

            public async Task<CustomerInfoResponse> Handle(GetCustomerInfoCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    return await apihelper.APIPostCall<CustomerInfoResponse, CustomerInfoRequest>("vend", request.Item);
                }
                catch (Exception ex)
                {
                    throw new APIException("ZETDC", ex.Message);
                }
            }
        }

    }
}
