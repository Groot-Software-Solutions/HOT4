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
    public class ItemCategoryGetQuery : IRequest<List<ItemCategory>>
    {
        public string OrderBy;
        public int Skip;
        public int Limit;

        public ItemCategoryGetQuery(string orderBy = "", int skip = 0, int limit = 0)
        {
            OrderBy = orderBy;
            Skip = skip;
            Limit = limit;
        }

        public class ItemCategoryGetQueryHandler : IRequestHandler<ItemCategoryGetQuery, List<ItemCategory>>
        {
            readonly ISageAPIService service;

            public ItemCategoryGetQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<ItemCategory>> Handle(ItemCategoryGetQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<ItemCategory>(request.OrderBy, request.Skip, request.Limit);
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-ItemCategory-Get", result.ErrorData);
            }
        }
    }
}