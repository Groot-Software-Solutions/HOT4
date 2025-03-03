namespace Hot.Application.Handlers.MessageHandlers.HelpHandlers
{
    public class ResetPinHelpMessageHandler: IHelpMessageHandler
    {
        private readonly IDbContext _dbContext;
        private readonly ILogger<ResetPinHelpMessageHandler> logger;

        public ResetPinHelpMessageHandler(IDbContext dbContext, ILogger<ResetPinHelpMessageHandler> logger)
        {
            _dbContext = dbContext;
            this.logger = logger;
        }

        public List<string> Name { get; set; } = new List<string>() { "PIN", "RESETPIN" };

        public async Task<OneOf<List<Template>, AppException>> HandleAsync(string MessageText)
        {
            var list = new List<Template>();
            var templateResults = await _dbContext.Templates.GetAsync((int)Templates.HelpResetPin);
            if (templateResults.IsT1) return templateResults.AsT1.LogAndReturnError(logger);
            list.Add(templateResults.AsT0);
            return list;
        }
    }
}
