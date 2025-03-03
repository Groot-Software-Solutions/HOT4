using MediatR;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sage.Application.Entities.CustomerReceipts
{
    public class CustomerReceiptGetQuery : IRequest<List<CustomerReceipt>>
    {
        public string OrderBy;
        public int Skip;
        public int Limit;

        public CustomerReceiptGetQuery(string orderBy = "", int skip = 0, int limit = 0)
        {
            OrderBy = orderBy;
            Skip = skip;
            Limit = limit;
        }

        public class CustomerReceiptGetQueryHandler : IRequestHandler<CustomerReceiptGetQuery, List<CustomerReceipt>>
        {
            readonly ISageAPIService service;

            public CustomerReceiptGetQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<CustomerReceipt>> Handle(CustomerReceiptGetQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<CustomerReceipt>(request.OrderBy, request.Skip, request.Limit);
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-CustomerReceipt-Get", result.ErrorData);
            }
        }
    }
}