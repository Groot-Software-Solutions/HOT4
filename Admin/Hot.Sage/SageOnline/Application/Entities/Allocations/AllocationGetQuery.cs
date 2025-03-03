using MediatR;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sage.Application.Entities.Allocations
{
    public class AllocationGetQuery : IRequest<List<Allocation>>
    {
        public string OrderBy;
        public int Skip;
        public int Limit;

        public AllocationGetQuery(string orderBy = "", int skip = 0, int limit = 0)
        {
            OrderBy = orderBy;
            Skip = skip;
            Limit = limit;
        }

        public class AllocationGetQueryHandler : IRequestHandler<AllocationGetQuery, List<Allocation>>
        {
            readonly ISageAPIService service;

            public AllocationGetQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<Allocation>> Handle(AllocationGetQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<Allocation>(request.OrderBy, request.Skip, request.Limit);
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-Allocation-Get", result.ErrorData);
            }
        }
    }
}