using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Application.Actions.ReconActions;
public record GetEconetZWLReconDifferencesQuery(DateTime startDate, DateTime EndDate) : IRequest<OneOf<List<EconetReconZWL>, AppException>>;
public class GetEconetZWLReconDifferencesQueryHandler : IRequestHandler<GetEconetZWLReconDifferencesQuery, OneOf<List<EconetReconZWL>, AppException>>
{
    public readonly IDbContext dbContext;
    public readonly ILogger<GetEconetZWLReconDifferencesQueryHandler> logger;

    public GetEconetZWLReconDifferencesQueryHandler(IDbContext dbContext, ILogger<GetEconetZWLReconDifferencesQueryHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<OneOf<List<EconetReconZWL>, AppException>> Handle(GetEconetZWLReconDifferencesQuery request, CancellationToken cancellationToken)
    {

        var EconeReconZWLResponse = await dbContext.EconetReconZWLs.GetEconetZWLReconResultAsync(request.startDate, request.EndDate);
        if (EconeReconZWLResponse.IsT1) return EconeReconZWLResponse.AsT1.LogAndReturnError(logger);
        var list = EconeReconZWLResponse.AsT0;
        return list;
    }
}
