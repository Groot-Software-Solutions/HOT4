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

    public class ReplyToSMS : IRequest<bool>
    {
        public ReplyToSMS(int originalSMSId, SMS reply)
        {
            OriginalSMSId = originalSMSId;
            Reply = reply;
        }

        public int OriginalSMSId { get; set; }
        public SMS Reply { get; set; }

        public class ReplyToSMSHandler : IRequestHandler<ReplyToSMS, bool>
        {
            readonly IDbContext _context;
            readonly IMailService _mailService;

            public ReplyToSMSHandler(IDbContext context, IMailService mailService)
            {
                _context = context;
                _mailService = mailService;
            }

            public async Task<bool> Handle(ReplyToSMS request, CancellationToken cancellationToken)
            {
                var sent = await _mailService.SendSMS(request.Reply); 
               
                request.Reply.StatusId = (sent ? Domain.Enums.SMSStatus.Success: Domain.Enums.SMSStatus.Failed);
                _ = await _context.SMSs.AddAsync(request.Reply);
                if (!sent) return false;
                 
                var originalsms = await _context.SMSs.GetAsync(request.OriginalSMSId);
                originalsms.StatusId = Domain.Enums.SMSStatus.Success;
                var updatedoriginal = await _context.SMSs.UpdateAsync(originalsms);
                if (!updatedoriginal) return false;

                 
                return true;
            }
        }
    }
}
