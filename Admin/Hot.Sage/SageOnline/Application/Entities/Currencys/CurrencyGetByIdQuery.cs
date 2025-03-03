using MediatR;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Domain.Entities;
using System.Threading.Tasks;
using System.Threading;

namespace Sage.Application.Entities.Currencys
{
    public class CurrencyGetByIdQuery : IRequest<Currency>
    {
        public int Id { get; set; }

        public CurrencyGetByIdQuery(int id)
        {
            Id = id;
        }

        public class CurrencyGetByIdQueryHandler : IRequestHandler<CurrencyGetByIdQuery, Currency>
        {
            readonly ISageAPIService service;

            public CurrencyGetByIdQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<Currency> Handle(CurrencyGetByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<Currency>(request.Id);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-Currency-GetById", result.ErrorData);
            }
        }
    }
}
