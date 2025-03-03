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

    public class AccountReceiptSaveCommand : IRequest<int>
    {
        public AccountReceiptSaveCommand(AccountReceipt item)
        {
            this.item = item;
        }

        public AccountReceipt item { get; set; }

        public class AccountReceiptSaveCommandHandler : IRequestHandler<AccountReceiptSaveCommand, int>
        {

            private ISageAPIService service;

            public AccountReceiptSaveCommandHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<int> Handle(AccountReceiptSaveCommand request, CancellationToken cancellationToken)
            { 
                var result = await service.Save(request.item);
                if (result.ValidResponse) return result.Result.AccountId;
                throw new SageAPIException("Sage-AccountReceipt-Save",result.ErrorData);

            }
        }
    }
}
