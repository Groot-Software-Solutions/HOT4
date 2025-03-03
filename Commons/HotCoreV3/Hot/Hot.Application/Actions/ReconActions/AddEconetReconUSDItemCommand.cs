using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Application.Actions.ReconActions;
public record AddEconetReconUSDItemCommand(List<EconetReconUSD> EconetItems) : IRequest<OneOf<bool, AppException>>;
public class AddEconetReconUSDItemCommandHandler : IRequestHandler<AddEconetReconUSDItemCommand, OneOf<bool, AppException>>
{
    public readonly IDbContext dbContext;
    public readonly ILogger<AddEconetReconUSDItemCommandHandler> logger;

    public AddEconetReconUSDItemCommandHandler(IDbContext dbContext, ILogger<AddEconetReconUSDItemCommandHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<OneOf<bool, AppException>> Handle(AddEconetReconUSDItemCommand request, CancellationToken cancellationToken)
    {
        foreach (var item in request.EconetItems)
        {
            var EconeReconUSDAddResponse = await dbContext.EconetReconUSDs.AddAsync(item);
            if (EconeReconUSDAddResponse.IsT1) return EconeReconUSDAddResponse.AsT1.LogAndReturnError(logger);
        }
        return true;
    }
}
