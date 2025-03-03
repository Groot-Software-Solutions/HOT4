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

    public class TaxTypeSaveCommand : IRequest<TaxType>
    {
        public TaxType item { get; set; }

        public TaxTypeSaveCommand(TaxType item)
        {
            this.item = item;
        }

        public class TaxTypeSaveCommandHandler : IRequestHandler<TaxTypeSaveCommand, TaxType>
        {
            readonly ISageAPIService service;

            public TaxTypeSaveCommandHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<TaxType> Handle(TaxTypeSaveCommand request, CancellationToken cancellationToken)
            {
                var result = await service.Save(request.item);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-TaxType-Save", result.ErrorData);
            }
        }
    }
}