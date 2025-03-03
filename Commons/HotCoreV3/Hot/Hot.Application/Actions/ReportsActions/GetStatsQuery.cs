namespace Hot.Application.Actions.ReportsActions;

public record GetStatsQuery(DateTime StartDate, DateTime EndDate, int ReportTypeId, long? NetworkID, int? AccountId) : IRequest<OneOf<List<PeriodicStatsResult>, AppException>>;
public class GetStatsQueryHandler : IRequestHandler<GetStatsQuery, OneOf<List<PeriodicStatsResult>, AppException>>
{
    private readonly IDbContext context;
    private readonly ILogger<GetStatsQueryHandler> logger;

    public GetStatsQueryHandler(IDbContext context, ILogger<GetStatsQueryHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<OneOf<List<PeriodicStatsResult>, AppException>> Handle(GetStatsQuery request, CancellationToken cancellationToken)
    {
        var response = await context.Report.GetStatsAsync(request.StartDate, request.EndDate, request.ReportTypeId, request.NetworkID, request.AccountId);
        if (response.IsT1) return response.AsT1.LogAndReturnError(logger);
        var statsList = response.AsT0;

        return statsList;
    }

}