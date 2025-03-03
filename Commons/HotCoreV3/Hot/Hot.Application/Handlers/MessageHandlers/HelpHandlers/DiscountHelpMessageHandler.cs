namespace Hot.Application.Handlers.MessageHandlers.HelpHandlers
{
    public class DiscountHelpMessageHandler : IHelpMessageHandler
    {
        private readonly IDbContext _dbContext;
        private readonly ILogger<DiscountHelpMessageHandler> logger;

        public DiscountHelpMessageHandler(IDbContext dbContext, ILogger<DiscountHelpMessageHandler> logger)
        {
            _dbContext = dbContext;
            this.logger = logger;
        }

        public List<string> Name { get; set; } = new List<string>() { "DISC", "DISCOUNT", "COM", "COMM", "COMMISSION" };

        public async Task<OneOf<List<Template>, AppException>> HandleAsync(string MessageText)
        {
            var list = new List<Template>();
            var templateResults = await _dbContext.Templates.GetAsync((int)Templates.HelpDiscount);
            if (templateResults.IsT1) return templateResults.AsT1.LogAndReturnError(logger);
            list.Add(templateResults.AsT0);
            return list;
        }
    }
}
