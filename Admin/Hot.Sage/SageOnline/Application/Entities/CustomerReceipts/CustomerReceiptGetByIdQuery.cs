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
    public class CustomerReceiptGetByIdQuery : IRequest<CustomerReceipt>
    {
        public int Id { get; set; }

        public CustomerReceiptGetByIdQuery(int id)
        {
            Id = id;
        }

        public class CustomerReceiptGetByIdQueryHandler : IRequestHandler<CustomerReceiptGetByIdQuery, CustomerReceipt>
        {
            readonly ISageAPIService service;

            public CustomerReceiptGetByIdQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<CustomerReceipt> Handle(CustomerReceiptGetByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<CustomerReceipt>(request.Id);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-CustomerReceipt-GetById", result.ErrorData);
            }
        }
    }
}