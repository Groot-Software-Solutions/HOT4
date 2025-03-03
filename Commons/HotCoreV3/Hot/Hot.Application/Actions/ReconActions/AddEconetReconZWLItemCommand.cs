using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Application.Actions.ReconActions;
public record AddEconetReconZWLItemCommand(List<EconetReconZWL> EconetItems) : IRequest<OneOf<bool, AppException>>;
public class AddEconetReconZWLItemCommandHandler : IRequestHandler<AddEconetReconZWLItemCommand, OneOf<bool, AppException>>
{
    public readonly IDbContext dbContext;
    public readonly ILogger<AddEconetReconZWLItemCommandHandler> logger;

    public AddEconetReconZWLItemCommandHandler(IDbContext dbContext, ILogger<AddEconetReconZWLItemCommandHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<OneOf<bool, AppException>> Handle(AddEconetReconZWLItemCommand request, CancellationToken cancellationToken)
    {
        var list = 0;
        foreach (var item in request.EconetItems)
        {
            var EconeReconZWLAddResponse = await dbContext.EconetReconZWLs.AddAsync(item);
            if (EconeReconZWLAddResponse.IsT1) return EconeReconZWLAddResponse.AsT1.LogAndReturnError(logger);           
        }
        return true;
    }
}
