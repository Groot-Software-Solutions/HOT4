using MediatR;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sage.Application.Entities.TaxTypes
{
    public class TaxTypeGetQuery : IRequest<List<TaxType>>
    {
        public string OrderBy;
        public int Skip;
        public int Limit;

        public TaxTypeGetQuery(string orderBy = "", int skip = 0, int limit = 0)
        {
            OrderBy = orderBy;
            Skip = skip;
            Limit = limit;
        }

        public class TaxTypeGetQueryHandler : IRequestHandler<TaxTypeGetQuery, List<TaxType>>
        {
            readonly ISageAPIService service;

            public TaxTypeGetQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<TaxType>> Handle(TaxTypeGetQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<TaxType>(request.OrderBy, request.Skip, request.Limit);
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-TaxType-Get", result.ErrorData);
            }
        }
    }
}