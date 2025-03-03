using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Application.Actions.ReconActions;


    public record GetTelecelZWLReconDifferencesQuery(DateTime startDate, DateTime EndDate) : IRequest<OneOf<List<TelecelReconZWL>, AppException>>;
    public class GetTelecelZWLReconDifferencesQueryHandler : IRequestHandler<GetTelecelZWLReconDifferencesQuery, OneOf<List<TelecelReconZWL>, AppException>>
    {
        public readonly IDbContext dbContext;
        public readonly ILogger<GetTelecelZWLReconDifferencesQueryHandler> logger;

        public GetTelecelZWLReconDifferencesQueryHandler(IDbContext dbContext, ILogger<GetTelecelZWLReconDifferencesQueryHandler> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<OneOf<List<TelecelReconZWL>, AppException>> Handle(GetTelecelZWLReconDifferencesQuery request, CancellationToken cancellationToken)
        {

            var EconeReconZWLResponse = await dbContext.TelecelReconZWLs.GetTelecelZWLReconResultAsync(request.startDate, request.EndDate);
            if (EconeReconZWLResponse.IsT1) return EconeReconZWLResponse.AsT1.LogAndReturnError(logger);
            var list = EconeReconZWLResponse.AsT0;
            return list;
        }
    }
