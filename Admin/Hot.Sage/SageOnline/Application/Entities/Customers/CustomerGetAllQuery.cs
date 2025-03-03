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
    public class CustomerGetAllQuery : IRequest<List<Customer>>
    {
       

        public class CustomerGetAllQueryHandler : IRequestHandler<CustomerGetAllQuery, List<Customer>>
        {
            readonly ISageAPIService service;

            public CustomerGetAllQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<Customer>> Handle(CustomerGetAllQuery request, CancellationToken cancellationToken)
            {
                var result = await service.GetAll<Customer>();
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-Customer-GetAll", result.ErrorData);
            } 
        }
    }
}