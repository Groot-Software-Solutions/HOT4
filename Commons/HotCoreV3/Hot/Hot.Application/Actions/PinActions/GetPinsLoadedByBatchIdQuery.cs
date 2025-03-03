namespace Hot.Application.Actions.PinActions
{

    public record GetPinsLoadedByBatchIdQuery(int BatchId) : IRequest<OneOf<List<PinStockModel>, AppException>>;
    public class GetPinsLoadedByBatchIdQueryHandler : IRequestHandler<GetPinsLoadedByBatchIdQuery, OneOf<List<PinStockModel>, AppException>>
    {
        private readonly IDbContext _context;
        private readonly ILogger<GetPinsLoadedByBatchIdQueryHandler> logger;


        public GetPinsLoadedByBatchIdQueryHandler(IDbContext context, ILogger<GetPinsLoadedByBatchIdQueryHandler> logger)
        {
            _context = context;
            this.logger = logger;
        }

        public async Task<OneOf<List<PinStockModel>, AppException>> Handle(GetPinsLoadedByBatchIdQuery request, CancellationToken cancellationToken)
        {
            //updated for suncing
            var response = await _context.Pins.StockLoadedInBatchAsync(request.BatchId);
            if (response.ResultOrNull() == null) return response.AsT1.LogAndReturnError("Pins Stock Error", logger);
            var LoadedPinsList = response.AsT0;
            return LoadedPinsList;
        }
    }
}