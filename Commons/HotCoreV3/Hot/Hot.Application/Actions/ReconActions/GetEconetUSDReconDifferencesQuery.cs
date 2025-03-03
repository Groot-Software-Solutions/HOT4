using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;  

namespace Hot.Application.Actions.ReconActions;
public record GetEconetUSDReconDifferencesQuery(DateTime StartDate, DateTime EndDate) : IRequest<OneOf<List<EconetReconUSD>, AppException>>;
public class GetEconetUSDReconDifferencesQueryHandler : IRequestHandler<GetEconetUSDReconDifferencesQuery, OneOf<List<EconetReconUSD>, AppException>>
{
    public readonly IDbContext dbContext;
    public readonly ILogger<GetEconetUSDReconDifferencesQueryHandler> logger;

    public GetEconetUSDReconDifferencesQueryHandler(IDbContext dbContext, ILogger<GetEconetUSDReconDifferencesQueryHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<OneOf<List<EconetReconUSD>, AppException>> Handle(GetEconetUSDReconDifferencesQuery request, CancellationToken cancellationToken)
    {

        var EconeReconUSDResponse = await dbContext.EconetReconUSDs.GetEconetUSDReconResultAsync(request.StartDate, request.EndDate);
        if (EconeReconUSDResponse.IsT1) return EconeReconUSDResponse.AsT1.LogAndReturnError(logger);
        var list = EconeReconUSDResponse.AsT0;
        return list;
    }
}
