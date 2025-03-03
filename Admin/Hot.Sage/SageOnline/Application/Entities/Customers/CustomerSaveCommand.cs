using MediatR;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Sage.Domain.Entities.Customer;

namespace Sage.Application.Entities.Customers
{

    public class CustomerSaveCommand : IRequest<Customer>
    {
        public Customer item { get; set; }

        public CustomerSaveCommand(Customer item)
        {
            this.item = item;
        }

        public class CustomerSaveCommandHandler : IRequestHandler<CustomerSaveCommand, Customer>
        {
            readonly ISageAPIService service;

            public CustomerSaveCommandHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<Customer> Handle(CustomerSaveCommand request, CancellationToken cancellationToken)
            {
                var item = new CustomerSaveRequest(request.item);
                var result = await service.Save<CustomerSaveRequest, Customer>(item);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-Customer-Save", result.ErrorData);
            }
        }
    }   

}
