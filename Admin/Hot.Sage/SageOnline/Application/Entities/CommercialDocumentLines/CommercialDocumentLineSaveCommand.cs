using MediatR;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sage.Application.Entities.CommercialDocumentLines
{

    public class CommercialDocumentLineSaveCommand : IRequest<CommercialDocumentLine>
    {
        public CommercialDocumentLine item { get; set; }

        public CommercialDocumentLineSaveCommand(CommercialDocumentLine item)
        {
            this.item = item;
        }

        public class CommercialDocumentLineSaveCommandHandler : IRequestHandler<CommercialDocumentLineSaveCommand, CommercialDocumentLine>
        {
            readonly ISageAPIService service;

            public CommercialDocumentLineSaveCommandHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<CommercialDocumentLine> Handle(CommercialDocumentLineSaveCommand request, CancellationToken cancellationToken)
            {
                var result = await service.Save(request.item);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-CommercialDocumentLine-Save", result.ErrorData);
            }
        }
    }
}