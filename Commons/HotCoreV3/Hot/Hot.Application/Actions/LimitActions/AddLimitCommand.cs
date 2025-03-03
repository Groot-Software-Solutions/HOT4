namespace Hot.Application.Actions.LimitActions;

public record AddLimitCommand(Limit limit) : IRequest<OneOf<int, AppException>>;
public class AddLimitCommandHandler : IRequestHandler<AddLimitCommand, OneOf<int, AppException>>
{
    private readonly IDbContext _context;
    private readonly ILogger<AddLimitCommandHandler> logger;

    public AddLimitCommandHandler(IDbContext context, ILogger<AddLimitCommandHandler> logger)
    {
        _context = context;
        this.logger = logger;
    }

    public async Task<OneOf<int, AppException>> Handle(AddLimitCommand request, CancellationToken cancellationToken)
    {
        var response = await _context.Limits.AddAsync(request.limit);
        if (response.IsT0)
        {
            return response.AsT0;
        }
        return response.AsT1.LogAndReturnError(logger);
    }
}
