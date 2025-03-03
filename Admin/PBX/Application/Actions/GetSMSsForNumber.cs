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

    public class GetSMSsForNumber : IRequest<List<SMS>>
    {
        public string Mobile { get; set; }

        public GetSMSsForNumber(string Mobile)
        {
            this.Mobile = Mobile;
        }

        public class GetSMSsForNumberQueryHandler : IRequestHandler<GetSMSsForNumber, List<SMS>>
        {
            private readonly IDbContext _context;

            public GetSMSsForNumberQueryHandler(IDbContext context)
            {
                _context = context;
            }

            public async Task<List<SMS>> Handle(GetSMSsForNumber request, CancellationToken cancellationToken)
            {
                // Get
                var entity = await _context.SMSs.SearchAsync(request.Mobile);
                if (entity == null)
                {
                    throw new NotFoundException(nameof(GetSMSsForNumber), request.Mobile);
                }

                return entity;
            }
        }
    }
}
