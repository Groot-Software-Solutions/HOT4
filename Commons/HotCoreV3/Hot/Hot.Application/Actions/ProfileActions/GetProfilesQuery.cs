namespace Hot.Application.Actions.ProfileActions;
public record GetProfilesQuery : IRequest<OneOf<List<Profile>, NotFoundException, AppException>>;
public class GetProfilesQueryHander : IRequestHandler<GetProfilesQuery, OneOf<List<Profile>, NotFoundException, AppException>>
{
    private readonly IDbContext _context;
    private readonly ILogger<GetProfilesQueryHander> _logger;

    public GetProfilesQueryHander(IDbContext context, ILogger<GetProfilesQueryHander> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<OneOf<List<Profile>, NotFoundException, AppException>> Handle(GetProfilesQuery request, CancellationToken cancellationToken)
    {
        var response = await _context.Profiles.ListAsync();
        if (response.ResultOrNull() == null) return response.AsT1.LogAndReturnError("Profiles listing error", _logger);
        var profileList = response.AsT0;
        if (!profileList.Any()) return new NotFoundException("Profiles", "");
        return profileList;
    }
}
