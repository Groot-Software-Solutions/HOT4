using Hot.Application.Actions.RechargeActions;
using Hot.Application.Actions.SMSActions;
using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Extensions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Models;
using Hot.Domain.Entities;
using Hot.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace Hot.Nyaradzo.Application.Actions;

[Obsolete("Please Use the Process Utility Command")]
public record ProcessNyaradzoPayment(long AccessId, string AccountNumber, decimal Amount, int Months, string TargetMobile)
    : IRequest<OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>>;

public class ProcessNyaradzoPaymentHandler : IRequestHandler<ProcessNyaradzoPayment, OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>>
{
    private readonly IRechargeHandlerFactory serviceFactory;
    private readonly IDbContext dbContext;
    private readonly ILogger<ProcessRechargeCommandHandler> logger;

    public ProcessNyaradzoPaymentHandler(IRechargeHandlerFactory serviceFactory, IDbContext context, ILogger<ProcessRechargeCommandHandler> logger)
    {
        this.serviceFactory = serviceFactory;
        this.dbContext = context;
        this.logger = logger;
    }

    public async Task<OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>> Handle(ProcessNyaradzoPayment request, CancellationToken cancellationToken)
    {
        var brandId = (int)Brands.Nyaradzo;

        if (serviceFactory.HasService(brandId) == false) return new UnsupportedBrandException(brandId, null, dbContext);
        var service = serviceFactory.GetService(brandId);

        var response = await ProcessRechargeBase.SetupRecharge(request.AccessId, brandId, request.Amount, request.AccountNumber, dbContext, logger);
        if (response.IsT0 == false) return ProcessRechargeBase.ReturnRechargeSetupError(response);
        var recharge = response.AsT0.Item1;
        var rechargePrepaid = response.AsT0.Item2;

        var rechargeResponse = await service.ProcessAsync(recharge, rechargePrepaid);
        if (rechargeResponse.IsT1) return rechargeResponse.AsT1.LogAndReturnError(logger);
        if (rechargeResponse.AsT0.Successful) await SendConfirmationSMStoDealerAsync(rechargeResponse.AsT0);
        return rechargeResponse.AsT0;
    }

    private async Task SendConfirmationSMStoDealerAsync(RechargeResult rechargeResponse)
    {
        try
        {
            var accessId = rechargeResponse.Recharge?.AccessId ?? 0;
            var accessResponse = await dbContext.Accesss.GetAsync(accessId);
            if (accessResponse.IsT0 == false) return;
            var access = accessResponse.AsT0;
            if (access.ChannelID is not ((byte)Channels.USSD or (byte)Channels.Sms)) return;

            var template = Templates.SuccessfulRechargePin_Dealer;
            var reply = TemplateExtensions.GetTemplate(dbContext, template);
            if (reply is null) return;

            reply.SetAmount((rechargeResponse.Recharge?.Amount) ?? 0);
            reply.SetBalance((rechargeResponse.WalletBalance) ?? 0);
            reply.SetMobile(rechargeResponse.Recharge?.Mobile ?? "");

            SMS sms = new()
            {
                Direction = false,
                Mobile = access.AccessCode,
                Priority = new Priority() { PriorityId = (byte)Priorities.Normal },
                State = new State() { StateID = (byte)States.Pending },
                SMSID_In = null,
                SMSText = reply.TemplateText,
            };

            var result = await dbContext.SMSs.AddAsync(sms);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error Sending Confirmation SMS - {message}", ex.Message);
        }
        return;
    }
}
 