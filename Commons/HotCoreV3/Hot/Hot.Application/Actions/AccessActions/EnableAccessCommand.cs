namespace Hot.Application.Actions.AccessActions;
public record EnableAccessCommand(long AccessId) : IRequest<OneOf<bool, NotFoundException, AppException>>;
    
    public class EnableAccessCommandHandler : IRequestHandler<EnableAccessCommand, OneOf<bool, NotFoundException, AppException>>
    {
        private readonly IDbContext _context;
        private readonly ILogger<EnableAccessCommandHandler> logger;

        public EnableAccessCommandHandler(IDbContext context, ILogger<EnableAccessCommandHandler> logger)
        {
            _context = context;
            this.logger = logger;
        }

        public async Task<OneOf<bool, NotFoundException, AppException>> Handle(EnableAccessCommand request, CancellationToken cancellationToken)
        {
            var accessResult = await _context.Accesss.GetAsync((int)request.AccessId);
            if (accessResult.IsT1)
            {
                if (accessResult.AsT1.IsNotFoundException()) return accessResult.AsT1.LogAndReturnError(logger);
                return new AppException("Enable Access Error", accessResult.AsT1);
            }
            var access = accessResult.AsT0;
            var accessRemoveResult = await _context.Accesss.UnDeleteAsync((int)access.AccessId);
            if (accessRemoveResult.IsT1) return accessRemoveResult.AsT1.LogAndReturnError(logger);

            return accessRemoveResult.IsT0;
        }
    }

