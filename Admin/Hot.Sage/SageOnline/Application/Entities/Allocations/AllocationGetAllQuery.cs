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
    public class AllocationGetAllQuery : IRequest<List<Allocation>>
    {
        


        public class AllocationGetAllQueryHandler : IRequestHandler<AllocationGetAllQuery, List<Allocation>>
        {
            readonly ISageAPIService service;

            public AllocationGetAllQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<Allocation>> Handle(AllocationGetAllQuery request, CancellationToken cancellationToken)
            {
                var result = await service.GetAll<Allocation>();
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-Allocation-GetAll", result.ErrorData);
            }
        }
    }
}