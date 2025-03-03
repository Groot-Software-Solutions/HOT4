namespace Hot.Application.Handlers.MessageHandlers.HelpHandlers
{
    public class RechargeHelpMessageHandler : IHelpMessageHandler
    {
        private readonly IDbContext _dbContext;
        private readonly ILogger<RechargeHelpMessageHandler> logger;

        public RechargeHelpMessageHandler(IDbContext dbContext, ILogger<RechargeHelpMessageHandler> logger)
        {
            _dbContext = dbContext;
            this.logger = logger;
        }

        public List<string> Name { get; set; } = new List<string>() { "RECHARGE", "HOT" };


        public async Task<OneOf<List<Template>, AppException>> HandleAsync(string MessageText)
        {
            var list = new List<Template>();
            var templateResults = await _dbContext.Templates.GetAsync((int)Templates.HelpRecharge);
            if (templateResults.IsT1) return templateResults.AsT1.LogAndReturnError(logger);
            list.Add(templateResults.AsT0);
            return list;
        }
    }
}
