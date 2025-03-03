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

    public class ItemNoteSaveCommand : IRequest<ItemNote>
    {
        public ItemNote item { get; set; }

        public ItemNoteSaveCommand(ItemNote item)
        {
            this.item = item;
        }

        public class ItemNoteSaveCommandHandler : IRequestHandler<ItemNoteSaveCommand, ItemNote>
        {
            readonly ISageAPIService service;

            public ItemNoteSaveCommandHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<ItemNote> Handle(ItemNoteSaveCommand request, CancellationToken cancellationToken)
            {
                var result = await service.Save(request.item);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-ItemNote-Save", result.ErrorData);
            }
        }
    }
}