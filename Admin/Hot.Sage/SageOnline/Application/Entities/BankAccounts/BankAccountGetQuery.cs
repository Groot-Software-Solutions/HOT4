using MediatR;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sage.Application.Entities.BankAccounts
{
    public class BankAccountGetQuery : IRequest<List<BankAccount>>
    {
        public string OrderBy;
        public int Skip;
        public int Limit;

        public BankAccountGetQuery(string orderBy = "", int skip = 0, int limit = 0)
        {
            OrderBy = orderBy;
            Skip = skip;
            Limit = limit;
        }

        public class BankAccountGetQueryHandler : IRequestHandler<BankAccountGetQuery, List<BankAccount>>
        {
            readonly ISageAPIService service;

            public BankAccountGetQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<BankAccount>> Handle(BankAccountGetQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<BankAccount>(request.OrderBy, request.Skip, request.Limit);
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-BankAccount-Get", result.ErrorData);
            }
        }
    }
}