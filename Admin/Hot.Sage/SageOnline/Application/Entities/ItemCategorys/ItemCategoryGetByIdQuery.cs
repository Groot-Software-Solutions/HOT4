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
    public class ItemCategoryGetByIdQuery : IRequest<ItemCategory>
    {
        public int Id { get; set; }

        public ItemCategoryGetByIdQuery(int id)
        {
            Id = id;
        }

        public class ItemCategoryGetByIdQueryHandler : IRequestHandler<ItemCategoryGetByIdQuery, ItemCategory>
        {
            readonly ISageAPIService service;

            public ItemCategoryGetByIdQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<ItemCategory> Handle(ItemCategoryGetByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<ItemCategory>(request.Id);
                if (result.ValidResponse) return result.Result;
                throw new SageAPIException("Sage-ItemCategory-GetById", result.ErrorData);
            }
        }
    }
}