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
    public class CustomerNoteGetByIdQuery : IRequest<CustomerNote>
    {
        public int Id { get; set; }

        public CustomerNoteGetByIdQuery(int id)
        {
            Id = id;
        }

        public class CustomerNoteGetByIdQueryHandler : IRequestHandler<CustomerNoteGetByIdQuery, CustomerNote>
        {
            readonly ISageAPIService service;

            public CustomerNoteGetByIdQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<CustomerNote> Handle(CustomerNoteGetByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<CustomerNote>(request.Id);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-CustomerNote-GetById", result.ErrorData);
            }
        }
    }
}