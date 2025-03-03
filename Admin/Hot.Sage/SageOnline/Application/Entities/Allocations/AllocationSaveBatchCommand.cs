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

    public class AllocationSaveBatchCommand : IRequest<List<Allocation>>
    {
        public List<Allocation> items { get; set; }

        public AllocationSaveBatchCommand(List<Allocation> items)
        {
            this.items = items;
        }

        public class AllocationSaveBatchCommandHandler : IRequestHandler<AllocationSaveBatchCommand, List<Allocation>>
        {
            readonly ISageAPIService service;

            public AllocationSaveBatchCommandHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<Allocation>> Handle(AllocationSaveBatchCommand request, CancellationToken cancellationToken)
            {
                var result = await service.SaveBatch(request.items);
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-Allocation-SaveBatch", result.ErrorData);
            }
        }
    }
}