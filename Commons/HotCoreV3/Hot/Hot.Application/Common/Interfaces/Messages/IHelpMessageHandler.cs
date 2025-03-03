namespace Hot.Application.Handlers.MessageHandlers.HelpHandlers
{
    public interface IHelpMessageHandler
    {
        public List<string> Name { get; set; }
        public Task<OneOf<List<Template>, AppException>> HandleAsync(string MessageText);

    }
}