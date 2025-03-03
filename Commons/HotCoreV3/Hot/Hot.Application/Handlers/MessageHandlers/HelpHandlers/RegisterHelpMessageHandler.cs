namespace Hot.Application.Handlers.MessageHandlers.HelpHandlers
{
    public class RegisterHelpMessageHandler : IHelpMessageHandler
    {
        private readonly IDbContext _dbContext;
        private readonly ILogger<RegisterHelpMessageHandler> logger;

        public RegisterHelpMessageHandler(IDbContext dbContext, ILogger<RegisterHelpMessageHandler> logger)
        {
            _dbContext = dbContext;
            this.logger = logger;
        }

        public List<string> Name { get; set; }  = new List<string>() { "REG", "REGISTER" };

        public async Task<OneOf<List<Template>, AppException>> HandleAsync(string MessageText)
        {
            var list = new List<Template>();
            var templateResults = await _dbContext.Templates.GetAsync((int)Templates.HelpRegister);
            if (templateResults.IsT1) return templateResults.AsT1.LogAndReturnError(logger);
            list.Add(templateResults.AsT0);
            return list;
        }
    }
}
