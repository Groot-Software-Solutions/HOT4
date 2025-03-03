

using Hot.Application.Actions.AccountActions;
using Hot.Application.Actions.SelfTopUpActions;
using Hot.Application.Actions.SMSActions;

namespace Hot.Application.Actions.RechargeActions;

public record RechargeReservationCompleteCommand(int AccessId, string AgentReference, bool Confirmed, string? ConfirmationData)
    : IRequest<OneOf<RechargeResult, ReservationNotFoundException, AppException>>;


public class RechargeReservationCompleteCommandHandler
    : IRequestHandler<RechargeReservationCompleteCommand, OneOf<RechargeResult, ReservationNotFoundException, AppException>>
{
    private readonly IDbContext dbContext;
    private readonly ILogger<RechargeReservationCompleteCommandHandler> logger;
    private readonly IMediator mediator;
    private readonly IRechargeHandlerFactory serviceFactory;

    public RechargeReservationCompleteCommandHandler(IDbContext dbContext, ILogger<RechargeReservationCompleteCommandHandler> logger, IMediator mediator, IRechargeHandlerFactory serviceFactory)
    {
        this.dbContext = dbContext;
        this.logger = logger;
        this.mediator = mediator;
        this.serviceFactory = serviceFactory;
    }

    public async Task<OneOf<RechargeResult, ReservationNotFoundException, AppException>> Handle(RechargeReservationCompleteCommand request, CancellationToken cancellationToken)
    {
        var webRequestResponse = await dbContext.WebRequests.GetAsync(request.AgentReference, request.AccessId);
        if (webRequestResponse.IsT1) return new ReservationNotFoundException("Reservation Not Found", $"Reservation for reference({request.AgentReference}) was not found");
        var webRequest = webRequestResponse.AsT0;

        var reservationResponse = await dbContext.Reservations.GetByRechargeIdAsync(webRequest.RechargeID ?? 0);
        if (reservationResponse.IsT1) return new ReservationNotFoundException("Reservation Failed", $"Failed to retrieve reservation for reference({request.AgentReference}) with RechargeId({webRequest.RechargeID}) was not found");
        var reservation = reservationResponse.AsT0;
        reservation.ConfirmationData = request.ConfirmationData;

        var rechargeResponse = await dbContext.Recharges.GetAsync(reservation.RechargeID);
        if (rechargeResponse.IsT1) return rechargeResponse.AsT1.LogAndReturnError(logger);
        var recharge = rechargeResponse.AsT0;

        var rechargePrepaidResponse = await dbContext.RechargePrepaids.GetAsync(reservation.RechargeID);
        if (rechargePrepaidResponse.IsT1) return rechargePrepaidResponse.AsT1.LogAndReturnError(logger);
        var rechargePrapaid = rechargePrepaidResponse.AsT0;



        if (request.Confirmed == false) return await CancelReservation(request, reservation, recharge, rechargePrapaid);
        if (reservation.StateId is not (byte)States.Pending or 5) return ReservationInCompletedState(reservation, recharge, rechargePrapaid);

        var service = serviceFactory.GetService(recharge.BrandId);

        var rechargeResultResponse = await service.ProcessAsync(recharge, rechargePrapaid);
        if (rechargeResultResponse.IsT1) return rechargeResultResponse.AsT1.LogAndReturnError(logger);

        var rechargeResult = rechargeResultResponse.AsT0;
        reservation.StateId = (byte)(rechargeResult.Successful ? 2 : 4);
        await dbContext.Reservations.UpdateAsync(reservation);

        if (rechargeResult.Successful)
        {
            await mediator.Send(
           new SendConfirmationSMStoCustomerCommand(reservation.NotificationNumber, recharge.Amount, rechargeResult.RechargePrepaid?.FinalBalance ?? 0)
           , cancellationToken);
        }
        else
        {
            var response = await HandleFailure(reservation, recharge, rechargeResult);
            rechargeResult.Message = response.IsT1
                    ? $"Reserved Recharge failed with following error.The transaction will be retried in 5 minutes - {rechargeResult.Message}"
                    : response.AsT0;
            if (response.IsT0)
            {
                rechargeResult.Successful = true;
                reservation.StateId = 6;
                await dbContext.Reservations.UpdateAsync(reservation);
            }
        }

        return rechargeResult;

    }

    private RechargeResult ReservationInCompletedState(Reservation reservation, Recharge recharge, RechargePrepaid rechargePrapaid)
    {
        var message = reservation.StateId switch
        {
            2 => "Reservation was completed and recharge was successful",
            3 => "The reservation was successfully cancelled previously, please submit a new reservation.",
            4 => "The reservation was completed but the recharge failed. The client will be credited in their Hot Recharge account",
            _ => "Reservation was already completed"
        };

        return new RechargeResult()
        {
            Recharge = recharge,
            RechargePrepaid = rechargePrapaid,
            Successful = recharge.StateId == 2,
            Message = message,
            WalletBalance = 0,
            Data = new { reservation.ConfirmationData }
        };
    }

    private async Task<OneOf<string, AppException>> HandleFailure(Reservation reservation, Recharge recharge, RechargeResult resultResult)
    {
        var accessResponse = await dbContext.Accesss.GetAsync(recharge.AccessId);
        var sourceAccess = accessResponse.AsT0;
        Access targetAccess = new();
        var targetAccountResponse = await dbContext.Accesss.SelectCodeAsync(reservation.NotificationNumber);
        if (targetAccountResponse.IsT0)
        {
            targetAccess = targetAccountResponse.AsT0;
        }
        else
        {
            var selftoptupAccountResponse = await RegisterSelfTopUpUser(reservation.NotificationNumber);
            if (selftoptupAccountResponse.IsT1) return selftoptupAccountResponse.AsT1.LogAndReturnError(logger);
            targetAccess = selftoptupAccountResponse.AsT0;
        }

        if (reservation.Currency == Currency.USD)
        {
            var result = await mediator.Send(new TransferWalletAirtimeBalanceCommand(sourceAccess.AccessCode, targetAccess.AccessCode, recharge.Amount));
            if (result.IsT1) return result.AsT1.LogAndReturnError(logger);
            return $"Reserved Recharge failed but funds of the value {recharge.Amount:#,##0.00} have been credited in the Hot recharge account of {reservation.TargetNumber}. They can try recharge later by dailing *180# using the same number";

        }
        else
        {
            var result = await mediator.Send(new TransferWalletAirtimeBalanceCommand(sourceAccess.AccessCode, targetAccess.AccessCode, recharge.Amount));
            if (result.IsT1) return result.AsT1.LogAndReturnError(logger);
            return $"Reserved Recharge failed but funds of the value {recharge.Amount:#,##0.00} have been credited in the Hot recharge account of {reservation.TargetNumber}. They can try recharge later by dailing *180# using the same number";
        }
    }
    private async Task<OneOf<Access, HotDbException>> RegisterSelfTopUpUser(string AccessCode)
    {
        var response = await mediator.Send(new AccountRegistrationCommand($"SelfTopUp-{AccessCode}", "SelftopUp", AccessCode, AccessCode, AccountHelper.RandomPin(), "", AccessCode, AccountType.Selftopup, "selftopup@hot.co.zw"));
        if (response.IsT1) throw response.AsT1.FailedToRegisterUser(AccessCode, logger);
        var accessResponse = await dbContext.Accesss.SelectCodeAsync(AccessCode);
        if (accessResponse.IsT1) throw accessResponse.AsT1.LogAndReturnError(logger);
        return accessResponse;
    }


    private async Task<OneOf<RechargeResult, ReservationNotFoundException, AppException>> CancelReservation(RechargeReservationCompleteCommand request, Reservation reservation, Recharge recharge, RechargePrepaid rechargePrapaid)
    {
        recharge.StateId = (byte)States.Failure;
        await dbContext.Recharges.UpdateAsync(recharge);

        reservation.StateId = (byte)States.Failure;
        await dbContext.Reservations.UpdateAsync(reservation);
        var result = new RechargeResult()
        {
            Successful = true,
            Message = "Reservation has been successfully cancelled",
            Recharge = recharge,
            RechargePrepaid = rechargePrapaid,
            WalletBalance = 0,
        };
        return result;
    }
}
