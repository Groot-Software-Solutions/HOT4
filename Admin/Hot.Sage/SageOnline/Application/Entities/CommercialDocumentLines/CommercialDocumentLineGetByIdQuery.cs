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
    public class CommercialDocumentLineGetByIdQuery : IRequest<CommercialDocumentLine>
    {
        public int Id { get; set; }

        public CommercialDocumentLineGetByIdQuery(int id)
        {
            Id = id;
        }

        public class CommercialDocumentLineGetByIdQueryHandler : IRequestHandler<CommercialDocumentLineGetByIdQuery, CommercialDocumentLine>
        {
            readonly ISageAPIService service;

            public CommercialDocumentLineGetByIdQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<CommercialDocumentLine> Handle(CommercialDocumentLineGetByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<CommercialDocumentLine>(request.Id);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-CommercialDocumentLine-GetById", result.ErrorData);
            }
        }
    }
}