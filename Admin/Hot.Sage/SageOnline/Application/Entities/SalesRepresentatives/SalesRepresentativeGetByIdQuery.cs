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
    public class SalesRepresentativeGetByIdQuery : IRequest<SalesRepresentative>
    {
        public int Id { get; set; }

        public SalesRepresentativeGetByIdQuery(int id)
        {
            Id = id;
        }

        public class SalesRepresentativeGetByIdQueryHandler : IRequestHandler<SalesRepresentativeGetByIdQuery, SalesRepresentative>
        {
            readonly ISageAPIService service;

            public SalesRepresentativeGetByIdQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<SalesRepresentative> Handle(SalesRepresentativeGetByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<SalesRepresentative>(request.Id);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-SalesRepresentative-GetById", result.ErrorData);
            }
        }
    }
}