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
    public class CommercialDocumentLineGetQuery : IRequest<List<CommercialDocumentLine>>
    {
        public string OrderBy;
        public int Skip;
        public int Limit;

        public CommercialDocumentLineGetQuery(string orderBy = "", int skip = 0, int limit = 0)
        {
            OrderBy = orderBy;
            Skip = skip;
            Limit = limit;
        }

        public class CommercialDocumentLineGetQueryHandler : IRequestHandler<CommercialDocumentLineGetQuery, List<CommercialDocumentLine>>
        {
            readonly ISageAPIService service;

            public CommercialDocumentLineGetQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<CommercialDocumentLine>> Handle(CommercialDocumentLineGetQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<CommercialDocumentLine>(request.OrderBy, request.Skip, request.Limit);
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-CommercialDocumentLine-Get", result.ErrorData);
            }
        }
    }
}