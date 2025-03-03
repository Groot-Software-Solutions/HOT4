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
    public class BankAccountGetByIdQuery : IRequest<BankAccount>
    {
        public int Id { get; set; }

        public BankAccountGetByIdQuery(int id)
        {
            Id = id;
        }

        public class BankAccountGetByIdQueryHandler : IRequestHandler<BankAccountGetByIdQuery, BankAccount>
        {
            readonly ISageAPIService service;

            public BankAccountGetByIdQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<BankAccount> Handle(BankAccountGetByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<BankAccount>(request.Id);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-BankAccount-GetById", result.ErrorData);
            }
        }
    }
}