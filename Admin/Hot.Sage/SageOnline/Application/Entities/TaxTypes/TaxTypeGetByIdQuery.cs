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
    public class TaxTypeGetByIdQuery : IRequest<TaxType>
    {
        public int Id { get; set; }

        public TaxTypeGetByIdQuery(int id)
        {
            Id = id;
        }

        public class TaxTypeGetByIdQueryHandler : IRequestHandler<TaxTypeGetByIdQuery, TaxType>
        {
            readonly ISageAPIService service;

            public TaxTypeGetByIdQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<TaxType> Handle(TaxTypeGetByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<TaxType>(request.Id);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-TaxType-GetById", result.ErrorData);
            }
        }
    }
}