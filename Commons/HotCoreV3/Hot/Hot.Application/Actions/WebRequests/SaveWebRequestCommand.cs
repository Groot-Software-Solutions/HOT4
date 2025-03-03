namespace Hot.Application.Actions.WebRequests;
public record SaveWebRequestCommand(WebRequest WebRequest): IRequest<OneOf<WebRequest,AppException>>;
public class SaveWebRequestCommandHandler : IRequestHandler<SaveWebRequestCommand, OneOf<WebRequest, AppException>>
{
    private readonly IDbContext dbContext;
    private readonly ILogger<SaveWebRequestCommandHandler> logger;

    public SaveWebRequestCommandHandler(IDbContext dbContext, ILogger<SaveWebRequestCommandHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<OneOf<WebRequest, AppException>> Handle(SaveWebRequestCommand request, CancellationToken cancellationToken)
    {
        var webRequest = request.WebRequest;

        var result = await dbContext.WebRequests.AddAsync(webRequest);
        if (result.IsT1) return result.AsT1.LogAndReturnError("Error Saving WebRequest", logger); 
        webRequest.WebID = result.AsT0;
        return webRequest;
    }
}
