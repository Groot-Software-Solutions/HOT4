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

    public class BankAccountSaveCommand : IRequest<BankAccount>
    {
        public BankAccount item { get; set; }

        public BankAccountSaveCommand(BankAccount item)
        {
            this.item = item;
        }

        public class BankAccountSaveCommandHandler : IRequestHandler<BankAccountSaveCommand, BankAccount>
        {
            readonly ISageAPIService service;

            public BankAccountSaveCommandHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<BankAccount> Handle(BankAccountSaveCommand request, CancellationToken cancellationToken)
            {
                var result = await service.Save(request.item);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-BankAccount-Save", result.ErrorData);
            }
        }
    }
}