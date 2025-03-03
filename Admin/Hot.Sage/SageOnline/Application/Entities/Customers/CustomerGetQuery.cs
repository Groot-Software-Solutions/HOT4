using MediatR;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sage.Application.Entities.Customers
{
    public class CustomerGetQuery : IRequest<List<Customer>>
    {
        public string OrderBy;
        public int Skip;
        public int Limit;

        public CustomerGetQuery(string orderBy = "", int skip = 0, int limit = 0)
        {
            OrderBy = orderBy;
            Skip = skip;
            Limit = limit;
        }

        public class CustomerGetQueryHandler : IRequestHandler<CustomerGetQuery, List<Customer>>
        {
            readonly ISageAPIService service;

            public CustomerGetQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<Customer>> Handle(CustomerGetQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<Customer>(request.OrderBy, request.Skip, request.Limit);
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-Customer-Get", result.ErrorData);
            }
        }
    }
}