namespace Hot.Application.Actions.WebRequests;

public record SearchWebRequestCommand(string AgentReference, int AccessId) : IRequest<OneOf<WebRequest,NotFoundException ,AppException>>;
public class SearchWebRequestCommandHandler : IRequestHandler<SearchWebRequestCommand, OneOf<WebRequest, NotFoundException, AppException>>
{
    private readonly IDbContext dbContext;
    private readonly ILogger<SearchWebRequestCommandHandler> logger;

    public SearchWebRequestCommandHandler(IDbContext dbContext, ILogger<SearchWebRequestCommandHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }
    public async Task<OneOf<WebRequest, NotFoundException, AppException>> Handle(SearchWebRequestCommand request, CancellationToken cancellationToken)
    {

        var result = await dbContext.WebRequests.GetAsync(request.AgentReference, request.AccessId);
         
            if (result.IsT1)
            {
            if (result.AsT1.IsNotFoundException()) return result.AsT1.ReturnNotFound("WebRequest",request.AgentReference);
                return result.AsT1.LogAndReturnError("Error Saving WebRequest", logger);
            }
        var webRequest = result.AsT0;
        return webRequest;
    }
}