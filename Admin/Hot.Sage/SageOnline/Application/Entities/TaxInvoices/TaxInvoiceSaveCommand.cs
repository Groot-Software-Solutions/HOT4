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

    public class TaxInvoiceSaveCommand : IRequest<TaxInvoice>
    {
        public TaxInvoice item { get; set; }

        public TaxInvoiceSaveCommand(TaxInvoice item)
        {
            this.item = item;
        }

        public class TaxInvoiceSaveCommandHandler : IRequestHandler<TaxInvoiceSaveCommand, TaxInvoice>
        {
            readonly ISageAPIService service;

            public TaxInvoiceSaveCommandHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<TaxInvoice> Handle(TaxInvoiceSaveCommand request, CancellationToken cancellationToken)
            {
                var result = await service.SaveSingle(request.item);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-TaxInvoice-Save", result.ErrorData);
            }
        }
    }
}