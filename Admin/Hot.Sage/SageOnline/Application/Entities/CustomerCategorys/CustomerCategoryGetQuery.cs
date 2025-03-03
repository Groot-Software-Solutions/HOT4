using MediatR;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sage.Application.Entities.CustomerCategorys
{
    public class CustomerCategoryGetQuery : IRequest<List<CustomerCategory>>
    {
        public string OrderBy;
        public int Skip;
        public int Limit;

        public CustomerCategoryGetQuery(string orderBy = "", int skip = 0, int limit = 0)
        {
            OrderBy = orderBy;
            Skip = skip;
            Limit = limit;
        }

        public class CustomerCategoryGetQueryHandler : IRequestHandler<CustomerCategoryGetQuery, List<CustomerCategory>>
        {
            readonly ISageAPIService service;

            public CustomerCategoryGetQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<CustomerCategory>> Handle(CustomerCategoryGetQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<CustomerCategory>(request.OrderBy, request.Skip, request.Limit);
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-CustomerCategory-Get", result.ErrorData);
            }
        }
    }
}