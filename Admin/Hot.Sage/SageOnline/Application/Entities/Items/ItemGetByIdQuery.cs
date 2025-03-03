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
    public class ItemGetByIdQuery : IRequest<Item>
    {
        public int Id { get; set; }

        public ItemGetByIdQuery(int id)
        {
            Id = id;
        }

        public class ItemGetByIdQueryHandler : IRequestHandler<ItemGetByIdQuery, Item>
        {
            readonly ISageAPIService service;

            public ItemGetByIdQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<Item> Handle(ItemGetByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<Item>(request.Id);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-Item-GetById", result.ErrorData);
            }
        }
    }
}