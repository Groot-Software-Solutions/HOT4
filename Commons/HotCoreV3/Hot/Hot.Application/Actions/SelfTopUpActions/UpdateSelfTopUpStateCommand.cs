

namespace Hot.Application.Actions.SelfTopUpActions;

public record UpdateSelfTopUpStateCommand(SelfTopUp SelfTopUp, BankTrx BankTrx)
    : IRequest<OneOf<SelfTopUpResult, AppException>>;

public class UpdateSelfTopUpStateCommandHandler : IRequestHandler<UpdateSelfTopUpStateCommand, OneOf<SelfTopUpResult, AppException>>
{
    private readonly IDbContext dbContext;
    private readonly ILogger<UpdateSelfTopUpStateCommandHandler> logger;

    public UpdateSelfTopUpStateCommandHandler(IDbContext dbContext, ILogger<UpdateSelfTopUpStateCommandHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<OneOf<SelfTopUpResult, AppException>> Handle(UpdateSelfTopUpStateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var TopUp = request.SelfTopUp;
            TopUp.StateId = (int)GetSelfTopStateFromBanktrx(request.BankTrx.BankTrxStateID);
            var response = await dbContext.SelfTopUps.UpdateAsync(TopUp);
            if (response.IsT1) return response.AsT1.LogAndReturnError(logger); 
            return new SelfTopUpResult(true, request.BankTrx, TopUp);
        }
        catch (Exception ex)
        {
            return ex.LogAndReturnError(logger);
        }

    }

    private static States GetSelfTopStateFromBanktrx(int BankState)
    {
        return BankState switch
        {
            0 => States.Pending,
            1 => States.PendingVerification,
            2 => States.Pending,
            6 => States.Pending,
            7 => States.Pending,
            9 => States.Pending,
            _ => States.Failure
        };
    }

}