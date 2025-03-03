using MediatR;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sage.Application.Entities.CustomerNotes
{

    public class CustomerNoteSaveCommand : IRequest<CustomerNote>
    {
        public CustomerNote item { get; set; }

        public CustomerNoteSaveCommand(CustomerNote item)
        {
            this.item = item;
        }

        public class CustomerNoteSaveCommandHandler : IRequestHandler<CustomerNoteSaveCommand, CustomerNote>
        {
            readonly ISageAPIService service;

            public CustomerNoteSaveCommandHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<CustomerNote> Handle(CustomerNoteSaveCommand request, CancellationToken cancellationToken)
            {
                var result = await service.Save(request.item);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-CustomerNote-Save", result.ErrorData);
            }
        }
    }
}