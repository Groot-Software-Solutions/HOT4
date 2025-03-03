using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Actions
{

    public class MarkSMSAsRead : IRequest<bool>
    {
        public MarkSMSAsRead(int id)
        {
            Id = id;
        }

        public int Id { get; set; }

        public class MarkSMSAsReadCommandHandler : IRequestHandler<MarkSMSAsRead, bool>
        {
            readonly IDbContext _context;

            public MarkSMSAsReadCommandHandler(IDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(MarkSMSAsRead request, CancellationToken cancellationToken)
            {

                //Update 
                var entity = await _context.SMSs.GetAsync(request.Id);
                if (entity == null)
                {
                    throw new NotFoundException(nameof(MarkSMSAsRead), request.Id);
                }
                entity.StatusId = Domain.Enums.SMSStatus.Read;
                var updated = await _context.SMSs.UpdateAsync(entity);
                if (updated)
                {
                    (await _context.SMSs.SearchAsync(entity.Mobile))
                        .Where(f => f.DirectionId == Domain.Enums.SMSDirection.In && f.StatusId == Domain.Enums.SMSStatus.New)
                        .ToList()
                        .ForEach(async s =>
                        {
                            s.StatusId = Domain.Enums.SMSStatus.Read;
                            await _context.SMSs.UpdateAsync(s);
                        });
                };

                return updated;
            }
        }
    }
}
