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
    public class ItemNoteGetQuery : IRequest<List<ItemNote>>
    {
        public string OrderBy;
        public int Skip;
        public int Limit;

        public ItemNoteGetQuery(string orderBy = "", int skip = 0, int limit = 0)
        {
            OrderBy = orderBy;
            Skip = skip;
            Limit = limit;
        }

        public class ItemNoteGetQueryHandler : IRequestHandler<ItemNoteGetQuery, List<ItemNote>>
        {
            readonly ISageAPIService service;

            public ItemNoteGetQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<ItemNote>> Handle(ItemNoteGetQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<ItemNote>(request.OrderBy, request.Skip, request.Limit);
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-ItemNote-Get", result.ErrorData);
            }
        }
    }
}