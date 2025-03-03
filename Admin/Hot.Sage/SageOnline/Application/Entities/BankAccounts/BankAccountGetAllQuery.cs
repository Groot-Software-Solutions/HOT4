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
    public class BankAccountGetAllQuery : IRequest<List<BankAccount>>
    {
        
        public class BankAccountGetAllQueryHandler : IRequestHandler<BankAccountGetAllQuery, List<BankAccount>>
        {
            readonly ISageAPIService service;

            public BankAccountGetAllQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<BankAccount>> Handle(BankAccountGetAllQuery request, CancellationToken cancellationToken)
            {
                var result = await service.GetAll<BankAccount>();
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-BankAccount-GetAll", result.ErrorData);
            }
        }
    }
}