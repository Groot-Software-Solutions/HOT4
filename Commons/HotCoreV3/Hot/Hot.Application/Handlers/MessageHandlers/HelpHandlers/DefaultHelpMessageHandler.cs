namespace Hot.Application.Handlers.MessageHandlers.HelpHandlers
{
    public class DefaultHelpMessageHandler : IHelpMessageHandler
    {
        private readonly IDbContext _dbContext;
        private readonly ILogger<DefaultHelpMessageHandler> logger;

        public DefaultHelpMessageHandler(IDbContext dbContext, ILogger<DefaultHelpMessageHandler> logger)
        {
            _dbContext = dbContext;
            this.logger = logger;
        }

        List<string> IHelpMessageHandler.Name { get; set; } = new List<string> { "" };

        public async Task<OneOf<List<Template>, AppException>> HandleAsync(string MessageText)
        {
            var list = new List<Template>();
            var templateResults = await _dbContext.Templates.GetAsync((int)Templates.HelpDefault);
            if (templateResults.IsT1) return templateResults.AsT1.LogAndReturnError(logger);
            list.Add(templateResults.AsT0);
            var rechargeResponse = await _dbContext.Templates.GetAsync((int)Templates.HelpRecharge);
            if (rechargeResponse.IsT1) return rechargeResponse.AsT1.LogAndReturnError(logger);
            list.Add(rechargeResponse.AsT0);
            return list;
        }
    }
}
