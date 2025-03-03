using MediatR;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sage.Application.Entities.Customers
{
    public class CustomerGetByIdQuery : IRequest<Customer>
    {
        public long Id { get; set; }

        public CustomerGetByIdQuery(long id)
        {
            Id = id;
        }

        public class CustomerGetByIdQueryHandler : IRequestHandler<CustomerGetByIdQuery, Customer>
        {
            readonly ISageAPIService service;

            public CustomerGetByIdQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<Customer> Handle(CustomerGetByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<Customer>(request.Id);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-Customer-GetById", result.ErrorData);
            }
        }
    }
}