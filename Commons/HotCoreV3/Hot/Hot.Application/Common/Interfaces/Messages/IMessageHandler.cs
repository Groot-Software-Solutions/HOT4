namespace Hot.Application.Common.Interfaces
{
    public interface ISMSMessageHandler
    { 
        public Task<List<SMS>?> HandleSMSAsync(SMS sms); 
        public List<SMS>? HandleSMS(SMS sms);

        public HotTypes HotType { get; }
    }
}
