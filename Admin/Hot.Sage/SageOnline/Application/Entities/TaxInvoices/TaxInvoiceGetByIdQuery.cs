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
    public class TaxInvoiceGetByIdQuery : IRequest<TaxInvoice>
    {
        public int Id { get; set; }

        public TaxInvoiceGetByIdQuery(int id)
        {
            Id = id;
        }

        public class TaxInvoiceGetByIdQueryHandler : IRequestHandler<TaxInvoiceGetByIdQuery, TaxInvoice>
        {
            readonly ISageAPIService service;

            public TaxInvoiceGetByIdQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<TaxInvoice> Handle(TaxInvoiceGetByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<TaxInvoice>(request.Id);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-TaxInvoice-GetById", result.ErrorData);
            }
        }
    }
}