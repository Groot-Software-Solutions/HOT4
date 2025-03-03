using MediatR;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Sage.Application.Entities.Currencys
{
    public class CurrencyGetAllQuery : IRequest<List<Currency>>
    {

        public class CurrencyGetAllQueryHandler : IRequestHandler<CurrencyGetAllQuery, List<Currency>>
        {
            readonly ISageAPIService service;

            public CurrencyGetAllQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<Currency>> Handle(CurrencyGetAllQuery request, CancellationToken cancellationToken)
            {
                var result = await service.GetAll<Currency>();
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-Currency-GetAll", result.ErrorData);
            }
        }
    }
}
