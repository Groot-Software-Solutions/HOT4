using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Actions
{

    public class CheckForNewSMSs : IRequest<Unit>
    {

        public class CheckForNewSMSsHandler : IRequestHandler<CheckForNewSMSs, Unit>
        {
            readonly IMailService mailService;

            public CheckForNewSMSsHandler(IMailService mailService)
            {
                this.mailService = mailService;
            }

            public async Task<Unit> Handle(CheckForNewSMSs request, CancellationToken cancellationToken)
            {
                try
                {
                    var newMessages = mailService.GetNewEmails();
                    if (newMessages.Count > 0)
                         await mailService.HandleMessages(newMessages);
                     
                    return Unit.Value;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());   
                    return Unit.Value;
                }



            }
        }
    }

}
