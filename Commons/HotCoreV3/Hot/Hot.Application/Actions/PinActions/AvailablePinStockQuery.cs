namespace Hot.Application.Actions.PinActions;
public record AvailablePinStockQuery : IRequest<OneOf<List<PinStockModel>, AppException>>;
        public class AvailablePinStockQueryHandler : IRequestHandler<AvailablePinStockQuery, OneOf<List<PinStockModel>, AppException>>
        {
            private readonly IDbContext context;
            private readonly ILogger<AvailablePinStockQueryHandler> logger;

            public AvailablePinStockQueryHandler(IDbContext context, ILogger<AvailablePinStockQueryHandler> logger)
            {
                this.context = context;
                this.logger = logger;
            }

            public async Task<OneOf<List<PinStockModel>, AppException>> Handle(AvailablePinStockQuery request, CancellationToken cancellationToken)
            {
                var response = await context.Pins.StockAsync();
                if (response.IsT1)
                {
                    logger.LogError("DB Query Error", response.AsT1);
                    return new AppException("Pin Stock Query Error", response.AsT1.Message);
                }
                return response.AsT0;
            }
        }