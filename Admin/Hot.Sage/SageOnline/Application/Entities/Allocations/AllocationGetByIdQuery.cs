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
    public class AllocationGetByIdQuery : IRequest<Allocation>
    {
        public int Id { get; set; }

        public AllocationGetByIdQuery(int id)
        {
            Id = id;
        }

        public class AllocationGetByIdQueryHandler : IRequestHandler<AllocationGetByIdQuery, Allocation>
        {
            readonly ISageAPIService service;

            public AllocationGetByIdQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<Allocation> Handle(AllocationGetByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<Allocation>(request.Id);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-Allocation-GetById", result.ErrorData);
            }
        }
    }
}