using MediatR;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sage.Application.Entities.CustomerReceipts
{

    public class CustomerReceiptSaveCommand : IRequest<CustomerReceipt>
    {
        public CustomerReceipt item { get; set; }

        public CustomerReceiptSaveCommand(CustomerReceipt item)
        {
            this.item = item;
        }

        public class CustomerReceiptSaveCommandHandler : IRequestHandler<CustomerReceiptSaveCommand, CustomerReceipt>
        {
            readonly ISageAPIService service;

            public CustomerReceiptSaveCommandHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<CustomerReceipt> Handle(CustomerReceiptSaveCommand request, CancellationToken cancellationToken)
            {
                var result = await service.SaveSingle(request.item);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-CustomerReceipt-Save", result.ErrorData);
            }
        }
    }
}