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

    public class AllocationSaveCommand : IRequest<Allocation>
    {
        public Allocation item { get; set; }

        public AllocationSaveCommand(Allocation item)
        {
            this.item = item;
        }

        public class AllocationSaveCommandHandler : IRequestHandler<AllocationSaveCommand, Allocation>
        {
            readonly ISageAPIService service;

            public AllocationSaveCommandHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<Allocation> Handle(AllocationSaveCommand request, CancellationToken cancellationToken)
            {
                var result = await service.Save(request.item);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-Allocation-Save", result.ErrorData);
            }
        }
    }
}