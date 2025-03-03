using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Application.Actions.ReconActions;



public record AddTelecelReconZWLItemCommand(List<TelecelReconZWL> TelecelItems) : IRequest<OneOf<bool, AppException>>;
public class AddTelecelReconZWLItemCommandHandler : IRequestHandler<AddTelecelReconZWLItemCommand, OneOf<bool, AppException>>
{
    public readonly IDbContext dbContext;
    public readonly ILogger<AddTelecelReconZWLItemCommandHandler> logger;

    public AddTelecelReconZWLItemCommandHandler(IDbContext dbContext, ILogger<AddTelecelReconZWLItemCommandHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<OneOf<bool, AppException>> Handle(AddTelecelReconZWLItemCommand request, CancellationToken cancellationToken)
    {

        foreach (var item in request.TelecelItems)
        {
            var TelecelReconZWLAddResponse = await dbContext.TelecelReconZWLs.AddAsync(item);
            if (TelecelReconZWLAddResponse.IsT1) return TelecelReconZWLAddResponse.AsT1.LogAndReturnError(logger);
        }
        return true;
    }
}
