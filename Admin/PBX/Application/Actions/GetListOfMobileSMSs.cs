using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Actions
{

    public class GetListOfMobileSMSs : IRequest<List<MobileSMSDetails>>
    {

        public class GetListOfMobileSMSsHandler : IRequestHandler<GetListOfMobileSMSs, List<MobileSMSDetails>>
        {
            private readonly IDbContext _context;

            public GetListOfMobileSMSsHandler(IDbContext context)
            {
                _context = context;
            }

            public async Task<List<MobileSMSDetails>> Handle(GetListOfMobileSMSs request, CancellationToken cancellationToken)
            {
                // Get
                var entity = (await _context.SMSs.ListAsync())
                    .GroupBy(f => f.Mobile)
                    .Select(g => new { Mobile = g.Key, Items = g.ToList() })
                    .ToList();
                if (entity == null)
                {
                    throw new NotFoundException(nameof(GetListOfMobileSMSs), "");
                }
                var result = entity.Select(m => new MobileSMSDetails
                {
                    Mobile = m.Mobile,
                    LastSMS = m.Items.Where(s => s.DirectionId == Domain.Enums.SMSDirection.In).OrderByDescending(s => s.Date).FirstOrDefault() ?? m.Items.OrderByDescending(s => s.Date).First(),
                    NumberofUnreadSMS = m.Items.Where(s => s.StatusId == Domain.Enums.SMSStatus.New).Count()
                }).ToList();

                return result;
            }
        }
    }
}
