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
    public class CommercialDocumentLineGetAllQuery : IRequest<List<CommercialDocumentLine>>
    {
        public int Id { get; set; }

        public CommercialDocumentLineGetAllQuery(int id)
        {
            Id = id;
        }

        public class CommercialDocumentLineGetAllQueryHandler : IRequestHandler<CommercialDocumentLineGetAllQuery, List<CommercialDocumentLine>>
        {
            readonly ISageAPIService service;

            public CommercialDocumentLineGetAllQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<CommercialDocumentLine>> Handle(CommercialDocumentLineGetAllQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<CommercialDocumentLine>(request.Id);
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-CommercialDocumentLine-GetAll", result.ErrorData);
            }
        }
    }
}