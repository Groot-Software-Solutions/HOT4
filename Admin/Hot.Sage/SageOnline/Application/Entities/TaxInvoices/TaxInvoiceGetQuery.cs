using MediatR;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sage.Application.Entities.TaxInvoices
{
    public class TaxInvoiceGetQuery : IRequest<List<TaxInvoice>>
    {
        public string OrderBy;
        public int Skip;
        public int Limit;

        public TaxInvoiceGetQuery(string orderBy = "", int skip = 0, int limit = 0)
        {
            OrderBy = orderBy;
            Skip = skip;
            Limit = limit;
        }

        public class TaxInvoiceGetQueryHandler : IRequestHandler<TaxInvoiceGetQuery, List<TaxInvoice>>
        {
            readonly ISageAPIService service;

            public TaxInvoiceGetQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<TaxInvoice>> Handle(TaxInvoiceGetQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<TaxInvoice>(request.OrderBy, request.Skip, request.Limit);
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-TaxInvoice-Get", result.ErrorData);
            }
        }
    }
}