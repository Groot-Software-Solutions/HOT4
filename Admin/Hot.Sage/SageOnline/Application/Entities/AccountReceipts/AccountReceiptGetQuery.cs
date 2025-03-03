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
    public class AccountReceiptGetQuery : IRequest<List<AccountReceipt>>
    {
        public string OrderBy;
        public int Skip;
        public int Limit; 

        public AccountReceiptGetQuery(string orderBy = "", int skip = 0, int limit = 0)
        {
            OrderBy = orderBy;
            Skip = skip;
            Limit = limit;
        }

        public class AccountReceiptGetQueryHandler : IRequestHandler<AccountReceiptGetQuery, List<AccountReceipt>>
        {
            readonly ISageAPIService service;

            public AccountReceiptGetQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<AccountReceipt>> Handle(AccountReceiptGetQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<AccountReceipt>(request.OrderBy, request.Skip,request.Limit);
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-AccountReceipt-Get", result.ErrorData);
            }
        }
    }
}