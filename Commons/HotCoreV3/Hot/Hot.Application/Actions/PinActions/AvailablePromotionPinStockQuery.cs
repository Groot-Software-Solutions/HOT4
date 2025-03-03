namespace Hot.Application.Actions.PinActions;
public record AvailablePromotionPinStockQuery:IRequest<OneOf<List<PinStockModel>,AppException>>;
        public class AvailablePromotionPinsQueryHandler : IRequestHandler<AvailablePromotionPinStockQuery, OneOf<List<PinStockModel>, AppException>>
        {
            private readonly IDbContext context;
            private readonly ILogger<AvailablePromotionPinsQueryHandler> logger;

            public AvailablePromotionPinsQueryHandler(IDbContext context, ILogger<AvailablePromotionPinsQueryHandler> logger)
            {
                this.context = context;
                this.logger = logger;
            }

            public async Task<OneOf<List<PinStockModel>, AppException>> Handle(AvailablePromotionPinStockQuery request, CancellationToken cancellationToken)
            {
                var response = await context.Pins.PromoStockAsync();
                if (response.IsT1)
                { 
                    logger.LogError("DB Query Error",response.AsT1);
                    return new AppException("Promo Query Error", response.AsT1.Message);
                }
                return response.AsT0; 
            }
        }
