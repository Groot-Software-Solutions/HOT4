using MediatR;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sage.Application.Entities.Items
{

    public class ItemSaveCommand : IRequest<Item>
    {
        public Item item { get; set; }

        public ItemSaveCommand(Item item)
        {
            this.item = item;
        }

        public class ItemSaveCommandHandler : IRequestHandler<ItemSaveCommand, Item>
        {
            readonly ISageAPIService service;

            public ItemSaveCommandHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<Item> Handle(ItemSaveCommand request, CancellationToken cancellationToken)
            {
                var result = await service.Save(request.item);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-Item-Save", result.ErrorData);
            }
        }
    }
}