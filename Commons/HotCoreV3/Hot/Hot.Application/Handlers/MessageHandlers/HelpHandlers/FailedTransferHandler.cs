namespace Hot.Application.Handlers.MessageHandlers.HelpHandlers
{
    public class FailedTransferHandler
    {
        private readonly IDbContext _dbContext;
        private readonly ILogger<FailedTransferHandler> logger;

        public FailedTransferHandler(IDbContext dbContext, ILogger<FailedTransferHandler> logger)
        {
            _dbContext = dbContext;
            this.logger = logger;
        }

        public List<string> Name { get; set; } = new List<string>() { "FailedTran" };

        public async Task<OneOf<List<Template>, AppException>> HandleAsync(string MessageText)
        {
            var list = new List<Template>();
            var templateResults = await _dbContext.Templates.GetAsync((int)Templates.FailedTransferMobile);
            if (templateResults.IsT1) return templateResults.AsT1.LogAndReturnError(logger);
            list.Add(templateResults.AsT0);
            return list;
        }
    }
}
