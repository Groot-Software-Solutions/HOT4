using MediatR;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sage.Application.Entities.CustomerCategorys
{
    public class CustomerCategoryGetByIdQuery : IRequest<CustomerCategory>
    {
        public int Id { get; set; }

        public CustomerCategoryGetByIdQuery(int id)
        {
            Id = id;
        }

        public class CustomerCategoryGetByIdQueryHandler : IRequestHandler<CustomerCategoryGetByIdQuery, CustomerCategory>
        {
            readonly ISageAPIService service;

            public CustomerCategoryGetByIdQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<CustomerCategory> Handle(CustomerCategoryGetByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<CustomerCategory>(request.Id);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-CustomerCategory-GetById", result.ErrorData);
            }
        }
    }
}