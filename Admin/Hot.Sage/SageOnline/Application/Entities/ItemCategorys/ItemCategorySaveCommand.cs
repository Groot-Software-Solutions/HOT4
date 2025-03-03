using MediatR;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sage.Application.Entities.ItemCategorys
{

    public class ItemCategorySaveCommand : IRequest<ItemCategory>
    {
        public ItemCategory item { get; set; }

        public ItemCategorySaveCommand(ItemCategory item)
        {
            this.item = item;
        }

        public class ItemCategorySaveCommandHandler : IRequestHandler<ItemCategorySaveCommand, ItemCategory>
        {
            readonly ISageAPIService service;

            public ItemCategorySaveCommandHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<ItemCategory> Handle(ItemCategorySaveCommand request, CancellationToken cancellationToken)
            {
                var result = await service.Save(request.item);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-ItemCategory-Save", result.ErrorData);
            }
        }
    }
}