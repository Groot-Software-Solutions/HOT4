using MediatR;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sage.Application.Entities.ItemNotes
{
    public class ItemNoteGetByIdQuery : IRequest<ItemNote>
    {
        public int Id { get; set; }

        public ItemNoteGetByIdQuery(int id)
        {
            Id = id;
        }

        public class ItemNoteGetByIdQueryHandler : IRequestHandler<ItemNoteGetByIdQuery, ItemNote>
        {
            readonly ISageAPIService service;

            public ItemNoteGetByIdQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<ItemNote> Handle(ItemNoteGetByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<ItemNote>(request.Id);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-ItemNote-GetById", result.ErrorData);
            }
        }
    }
}