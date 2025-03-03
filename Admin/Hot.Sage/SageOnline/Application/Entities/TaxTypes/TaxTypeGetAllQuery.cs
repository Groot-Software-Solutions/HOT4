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
    public class TaxTypeGetAllQuery : IRequest<List<TaxType>>
    {
    
        public class TaxTypeGetAllQueryHandler : IRequestHandler<TaxTypeGetAllQuery, List<TaxType>>
        {
            readonly ISageAPIService service;

            public TaxTypeGetAllQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<TaxType>> Handle(TaxTypeGetAllQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<TaxType>();
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-TaxType-GetAll", result.ErrorData);
            }
        }
    }
}