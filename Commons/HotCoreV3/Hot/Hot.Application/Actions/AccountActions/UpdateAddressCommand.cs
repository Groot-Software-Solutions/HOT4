namespace Hot.Application.Actions.AccountActions;
public record UpdateAddressCommand(Address Address) : IRequest<OneOf<bool, NotFoundException, AppException>>;

public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, OneOf<bool, NotFoundException, AppException>>
{
    private readonly IDbContext _context;
    private readonly ILogger<UpdateAddressCommandHandler> _logger;

    public UpdateAddressCommandHandler(IDbContext context, ILogger<UpdateAddressCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<OneOf<bool, NotFoundException, AppException>> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var updateresult = await _context.Addresses.UpdateAsync(request.Address);
        if (updateresult.IsT1)
        {
            if (updateresult.AsT1.IsNotFoundException())
                return updateresult.AsT1.ReturnNotFound("Address", request.Address.AccountID.ToString());
            updateresult.AsT1.LogAndReturnError(_logger);
        } 
        return updateresult.AsT0; 
    }
}
