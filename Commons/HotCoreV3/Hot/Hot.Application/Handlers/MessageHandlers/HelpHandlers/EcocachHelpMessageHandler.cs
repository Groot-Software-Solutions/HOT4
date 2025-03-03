namespace Hot.Application.Handlers.MessageHandlers.HelpHandlers
{
    public class EcocachHelpMessageHandler : IHelpMessageHandler
    {
        private readonly IDbContext _dbContext;
        private readonly ILogger<EcocachHelpMessageHandler> logger;

        public EcocachHelpMessageHandler(IDbContext dbContext, ILogger<EcocachHelpMessageHandler> logger)
        {
            _dbContext = dbContext;
            this.logger = logger;
        }

        public List<string> Name { get; set; } = new List<string>() { "ECO", "ECOCASH", "EC0", "EC0CASH" };

        public async Task<OneOf<List<Template>, AppException>> HandleAsync(string MessageText)
        {
            var list = new List<Template>();
            var templateResults = await _dbContext.Templates.GetAsync((int)Templates.HelpEcoCash);
            if (templateResults.IsT1) return templateResults.AsT1.LogAndReturnError(logger);
            list.Add(templateResults.AsT0);
            return list;
        }
    }
}
