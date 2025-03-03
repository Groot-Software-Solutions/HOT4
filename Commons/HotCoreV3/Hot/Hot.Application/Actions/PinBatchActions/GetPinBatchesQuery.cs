using AutoMapper;

namespace Hot.Application.Actions.PinBatchActions;
public record GetPinBatchesQuery(byte PinBatchTypeId) : IRequest<OneOf<List<PinBatchDetailedModel>, AppException>>;
        public class GetPinBatchesQueryHandler : IRequestHandler<GetPinBatchesQuery, OneOf<List<PinBatchDetailedModel>, AppException>>
        {
            private readonly IDbContext _context;
            private readonly ILogger<GetPinBatchesQueryHandler> logger;
            private readonly IMapper mapper;

            public GetPinBatchesQueryHandler(IDbContext context, ILogger<GetPinBatchesQueryHandler> logger, IMapper mapper)
            {
                _context = context;
                this.logger = logger;
                this.mapper = mapper;
            }

            public async Task<OneOf<List<PinBatchDetailedModel>, AppException>> Handle(GetPinBatchesQuery request, CancellationToken cancellationToken)
            {
                List<PinBatch> pinbatches = new();
                List<PinBatchType> pinbatchtypes = new();

                var pinbatchresponse = await _context.PinBatches.ListAsync(request.PinBatchTypeId);
                if (pinbatchresponse.ResultOrNull() == null) return pinbatchresponse.AsT1.LogAndReturnError("PinBatches Listing Error", logger);
                pinbatches = pinbatchresponse.AsT0;

                var pinbatchtyperesponse = await _context.PinBatchTypes.ListAsync();
                if (pinbatchtyperesponse.ResultOrNull() == null) return pinbatchtyperesponse.AsT1.LogAndReturnError("PinBatch Types Listing Error", logger);
                pinbatchtypes = pinbatchtyperesponse.AsT0;

                var list = pinbatches.Select(p =>
                {
                    var pinbatch = mapper.Map<PinBatchDetailedModel>(p);
                    pinbatch.PinBatchType = pinbatch.GetPinBatchType(pinbatchtypes);
                    return pinbatch;
                }).ToList();

                return list;
            }
        }
