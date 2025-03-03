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

    public class CustomerCategorySaveCommand : IRequest<CustomerCategory>
    {
        public CustomerCategory item { get; set; }

        public CustomerCategorySaveCommand(CustomerCategory item)
        {
            this.item = item;
        }

        public class CustomerCategorySaveCommandHandler : IRequestHandler<CustomerCategorySaveCommand, CustomerCategory>
        {
            readonly ISageAPIService service;

            public CustomerCategorySaveCommandHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<CustomerCategory> Handle(CustomerCategorySaveCommand request, CancellationToken cancellationToken)
            {
                var result = await service.Save(request.item);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-CustomerCategory-Save", result.ErrorData);
            }
        }
    }
}