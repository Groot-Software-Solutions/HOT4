namespace Hot.Application.Actions.PinActions;
public record AccountHasPurchasedPromotionPinQuery(long AccountId) : IRequest<OneOf<bool,AppException>>;

        public class AccountHasPurchasedPromoPinQueryHandler : IRequestHandler<AccountHasPurchasedPromotionPinQuery, OneOf<bool, AppException>>
        {
            private readonly IDbContext context;
            private readonly ILogger<AccountHasPurchasedPromoPinQueryHandler> logger;

            public AccountHasPurchasedPromoPinQueryHandler(IDbContext context, ILogger<AccountHasPurchasedPromoPinQueryHandler> logger)
            {
                this.context = context;
                this.logger = logger;
            }

            public async Task<OneOf<bool, AppException>> Handle(AccountHasPurchasedPromotionPinQuery request, CancellationToken cancellationToken)
            {
                var response =  await context.Pins.PromoHasPurchasedAsync(request.AccountId);
                if (response.IsT1)
                {
                    logger.LogError("DB Error", response.AsT1);
                    return new AppException("Query Promotion has been redeemed error", response.AsT1.Message);
                }
                return response.AsT0;
            }
        }
