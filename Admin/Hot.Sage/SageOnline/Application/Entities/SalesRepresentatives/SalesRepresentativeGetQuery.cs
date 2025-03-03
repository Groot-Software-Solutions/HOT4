using MediatR;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sage.Application.Entities.SalesRepresentatives
{
    public class SalesRepresentativeGetQuery : IRequest<List<SalesRepresentative>>
    {
        public string OrderBy;
        public int Skip;
        public int Limit;

        public SalesRepresentativeGetQuery(string orderBy = "", int skip = 0, int limit = 0)
        {
            OrderBy = orderBy;
            Skip = skip;
            Limit = limit;
        }

        public class SalesRepresentativeGetQueryHandler : IRequestHandler<SalesRepresentativeGetQuery, List<SalesRepresentative>>
        {
            readonly ISageAPIService service;

            public SalesRepresentativeGetQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<SalesRepresentative>> Handle(SalesRepresentativeGetQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<SalesRepresentative>(request.OrderBy, request.Skip, request.Limit);
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-SalesRepresentative-Get", result.ErrorData);
            }
        }
    }
}