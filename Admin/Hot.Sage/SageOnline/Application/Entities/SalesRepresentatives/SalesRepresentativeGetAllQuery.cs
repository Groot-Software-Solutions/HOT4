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
    public class SalesRepresentativeGetAllQuery : IRequest<List<SalesRepresentative>>
    {
       

        public class SalesRepresentativeGetAllQueryHandler : IRequestHandler<SalesRepresentativeGetAllQuery, List<SalesRepresentative>>
        {
            readonly ISageAPIService service;

            public SalesRepresentativeGetAllQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<SalesRepresentative>> Handle(SalesRepresentativeGetAllQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<SalesRepresentative>();
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-SalesRepresentative-GetAll", result.ErrorData);
            }
        }
    }
}
