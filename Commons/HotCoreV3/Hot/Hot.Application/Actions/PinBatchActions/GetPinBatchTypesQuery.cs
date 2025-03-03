namespace Hot.Application.Actions.PinBatchActions;
public record GetPinBatchTypesQuery : IRequest<OneOf<List<PinBatchType>, AppException>>;
        public class GetPinBatchTypesQueryHandler : IRequestHandler<GetPinBatchTypesQuery, OneOf<List<PinBatchType>, AppException>>
        {
            private readonly IDbContext _context;
            private readonly ILogger<GetPinBatchTypesQueryHandler> logger;


            public GetPinBatchTypesQueryHandler(IDbContext context, ILogger<GetPinBatchTypesQueryHandler> logger)
            {
                _context = context;
                this.logger = logger;
               
            }

            public async Task<OneOf<List<PinBatchType>, AppException>> Handle(GetPinBatchTypesQuery request, CancellationToken cancellationToken)
            {
                var pinbatchtyperesponse = await _context.PinBatchTypes.ListAsync();
                if (pinbatchtyperesponse.ResultOrNull() == null) return pinbatchtyperesponse.AsT1.LogAndReturnError("PinBatch listing error", logger);
                var pinbatchtypes = pinbatchtyperesponse.AsT0;
                return pinbatchtypes;
            }
        }
