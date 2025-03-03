using Microsoft.Extensions.Logging;
using System;

namespace Hot.Application.Actions.RechargeActions;

public record ProcessRechargeAirtimeWithIdentifyCommand(string TargetMobile, decimal Amount, long AccessId, string? CustomSMS = null)
    : IRequest<OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>>;


public class ProcessRechargeAirtimeWithIdentifyCommandHandler : IRequestHandler<ProcessRechargeAirtimeWithIdentifyCommand, OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException,
        UnsupportedBrandException, AppException>>
{
    private readonly IDbContext context;
    private readonly ILogger<ProcessRechargeAirtimeWithIdentifyCommandHandler> logger;
    private readonly IMediator mediator;

    public ProcessRechargeAirtimeWithIdentifyCommandHandler(IDbContext context, ILogger<ProcessRechargeAirtimeWithIdentifyCommandHandler> logger, IMediator mediator)
    {
        this.context = context;
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task<OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>> Handle(ProcessRechargeAirtimeWithIdentifyCommand request, CancellationToken cancellationToken)
    {

        var brandResponse = await ProcessRechargeBase.GetBrandId(request.TargetMobile, context, logger);
        if (brandResponse.IsT1) return brandResponse.AsT1;
        var brandId = brandResponse.AsT0;

        return await mediator.Send(new ProcessRechargeAirtimeCommand((Brands)brandId, request.TargetMobile, request.Amount, request.AccessId,request.CustomSMS),cancellationToken);
    }
}

