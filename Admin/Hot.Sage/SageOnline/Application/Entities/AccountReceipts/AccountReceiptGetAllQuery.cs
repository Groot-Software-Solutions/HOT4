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

    public class AccounReceiptGetAllQuery : IRequest<List<AccountReceipt>>
    { 

        public class AccounReceiptGetAllQueryHandler : IRequestHandler<AccounReceiptGetAllQuery, List<AccountReceipt>>
        {
             readonly ISageAPIService service;

            public AccounReceiptGetAllQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<AccountReceipt>> Handle(AccounReceiptGetAllQuery request, CancellationToken cancellationToken)
            { 
                var result = await service.GetAll<AccountReceipt>();
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-AccountReceipt-GetAll", result.ErrorData);
            }
        }
    }
}
