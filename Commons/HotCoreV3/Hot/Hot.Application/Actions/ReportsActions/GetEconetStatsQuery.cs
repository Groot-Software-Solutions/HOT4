namespace Hot.Application.Actions.ReportsActions;

public record GetEconetStatsQuery(DateTime StartDate, DateTime EndDate) : IRequest<OneOf<List<EconetStatsResult>, AppException>>;
public class GetEconetStatsQueryHandler : IRequestHandler<GetEconetStatsQuery, OneOf<List<EconetStatsResult>, AppException>>
{
    private readonly IDbContext context;
    private readonly ILogger<GetEconetStatsQueryHandler> logger;

    public GetEconetStatsQueryHandler(IDbContext context, ILogger<GetEconetStatsQueryHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<OneOf<List<EconetStatsResult>, AppException>> Handle(GetEconetStatsQuery request, CancellationToken cancellationToken)
    {
        var response = await context.Report.GetEconetStatsAsync(request.StartDate, request.EndDate);
        if (response.IsT1) return response.AsT1.LogAndReturnError(logger);
        var stats = response.AsT0;

        return stats;
    }

}
