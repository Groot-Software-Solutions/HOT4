namespace Hot.Application.Actions.PinActions;
public record GetPinStockQuery : IRequest<OneOf<List<PinStockModel>, AppException>>;
        public class GetPinStockQueryHandler : IRequestHandler<GetPinStockQuery, OneOf<List<PinStockModel>, AppException>>
        {
            private readonly IDbContext _context;
            private readonly ILogger<GetPinStockQueryHandler> logger;
           

            public GetPinStockQueryHandler(IDbContext context, ILogger<GetPinStockQueryHandler> logger )
            {
                _context = context;
                this.logger = logger; 
            }

            public async Task<OneOf<List<PinStockModel>, AppException>> Handle(GetPinStockQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.Pins.StockAsync();
                if (response.ResultOrNull() == null) return response.AsT1.LogAndReturnError("Pins Stock Error", logger);
                var pinstocklist = response.AsT0;
                return pinstocklist;
            }
        }