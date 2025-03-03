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

    public class SalesRepresentativeSaveCommand : IRequest<SalesRepresentative>
    {
        public SalesRepresentative item { get; set; }

        public SalesRepresentativeSaveCommand(SalesRepresentative item)
        {
            this.item = item;
        }

        public class SalesRepresentativeSaveCommandHandler : IRequestHandler<SalesRepresentativeSaveCommand, SalesRepresentative>
        {
            readonly ISageAPIService service;

            public SalesRepresentativeSaveCommandHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<SalesRepresentative> Handle(SalesRepresentativeSaveCommand request, CancellationToken cancellationToken)
            {
                var result = await service.Save(request.item);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-SalesRepresentative-Save", result.ErrorData);
            }
        }
    }
}