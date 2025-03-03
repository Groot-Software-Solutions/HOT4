using Hot.Application.Actions.SMSActions;

namespace Hot.Application.Actions.PinActions;
public record PurchasePromoPinCommand(string AccessCode, int BrandId, decimal Value, string Mobile) : IRequest<OneOf<List<Pin>, AppException>>;
        public class PurchasePromoPinCommandHandler :  IRequestHandler<PurchasePromoPinCommand, OneOf<List<Pin>, AppException>>
        {
            private readonly IDbContext context;
            private readonly ILogger<PurchasePromoPinCommandHandler> logger;
            private readonly IMediator mediator;

            public PurchasePromoPinCommandHandler(IDbContext context, ILogger<PurchasePromoPinCommandHandler> logger, IMediator mediator)
            {
                this.context = context;
                this.logger = logger;
                this.mediator = mediator;
            }

            public async Task<OneOf<List<Pin>, AppException>> Handle(PurchasePromoPinCommand request, CancellationToken cancellationToken)
            {
                var response  = await context.Pins.PromoRechargeAsync(request.AccessCode, request.BrandId, request.Value, 1, request.Mobile);
                if (response.IsT1) return CommandHelper.ReturnDbException(response.AsT1, logger);
                var result = response.AsT0;
                _ = mediator.Send(new SendPinToDealerSMSCommand(result.First(), request.Mobile), cancellationToken); 
                return result;
            }
        }
