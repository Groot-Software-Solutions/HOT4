using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Actions
{ 
    public class GetRecentSMSs : IRequest<List<SMS>>
    {
        
        public class GetRecentSMSsHandler : IRequestHandler<GetRecentSMSs, List<SMS>>
        {
            readonly IDbContext _context;

            public GetRecentSMSsHandler(IDbContext context)
            {
                _context = context;
            }

            public async Task<List<SMS>> Handle(GetRecentSMSs request, CancellationToken cancellationToken)
            {
                // Get
                var entity = await _context.SMSs.ListAsync();
                if (entity == null)
                {
                    throw new NotFoundException(nameof(GetSMSsForNumber), "");
                }

                return entity;
            }
        }
    }
}
