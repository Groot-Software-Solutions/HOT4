using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Application.Actions.AddressActions;
public record GetAddressByAccountIdQuery(long AccountId) : IRequest<OneOf<Address, NotFoundException, AppException>>;

public class GetAddressByAccountIdQueryHandler : IRequestHandler<GetAddressByAccountIdQuery, OneOf<Address, NotFoundException, AppException>>
{
    private readonly IDbContext _context;
    private readonly ILogger<GetAddressByAccountIdQueryHandler> _logger;

    public GetAddressByAccountIdQueryHandler(IDbContext context, ILogger<GetAddressByAccountIdQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<OneOf<Address, NotFoundException, AppException>> Handle(GetAddressByAccountIdQuery request, CancellationToken cancellationToken)
    {

        var addressResponse = await _context.Addresses.GetAsync((int)request.AccountId);
        if (addressResponse.ResultOrNull() == null) return addressResponse.AsT1.LogAndReturnError(addressResponse.AsT1.Message, _logger);
        var address = addressResponse.ResultOrNull() ?? new Address();

        return address; ;
    }

}
