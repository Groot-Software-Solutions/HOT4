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
    public class CustomerNoteGetQuery : IRequest<List<CustomerNote>>
    {
        public string OrderBy;
        public int Skip;
        public int Limit;

        public CustomerNoteGetQuery(string orderBy = "", int skip = 0, int limit = 0)
        {
            OrderBy = orderBy;
            Skip = skip;
            Limit = limit;
        }

        public class CustomerNoteGetQueryHandler : IRequestHandler<CustomerNoteGetQuery, List<CustomerNote>>
        {
            readonly ISageAPIService service;

            public CustomerNoteGetQueryHandler(ISageAPIService service)
            {
                this.service = service;
            }

            public async Task<List<CustomerNote>> Handle(CustomerNoteGetQuery request, CancellationToken cancellationToken)
            {
                var result = await service.Get<CustomerNote>(request.OrderBy, request.Skip, request.Limit);
                if (result.ValidResponse) return result.Results;
                throw new SageAPIException("Sage-CustomerNote-Get", result.ErrorData);
            }
        }
    }
}