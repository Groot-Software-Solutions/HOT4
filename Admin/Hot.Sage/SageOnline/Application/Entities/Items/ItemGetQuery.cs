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
    public class ItemGetQuery : IRequest<List<Item>>
    {
        public string OrderBy;
        public int Skip;
        public int Limit;

        public ItemGetQuery(string orderBy = "", int skip = 0, int limit = 0)
        {
            OrderBy = orderBy;
            Skip = skip;
            Limit = limit;
        }

        public class ItemGetQueryHandler : IRequestHandler<ItemGetQuery, List<Item>>
        {
            readonly ISageAPIService service;

            public ItemGetQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<Item>> Handle(ItemGetQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<Item>(request.OrderBy, request.Skip, request.Limit);
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-Item-Get", result.ErrorData);
            }
        }
    }
}