  using MediatR;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sage.Application.Entities.AccountReceipts
{
    public class AccountReceiptGetByIdQuery : IRequest<AccountReceipt>
    {
        public int Id { get; set; }

        public AccountReceiptGetByIdQuery(int id)
        {
            Id = id;
        }

        public class AccountReceiptGetByIdQueryHandler : IRequestHandler<AccountReceiptGetByIdQuery, AccountReceipt>
        {
            readonly ISageAPIService service;

            public AccountReceiptGetByIdQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<AccountReceipt> Handle(AccountReceiptGetByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<AccountReceipt>(request.Id);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-AccountReceipt-GetById", result.ErrorData);
            }
        }
    }
}