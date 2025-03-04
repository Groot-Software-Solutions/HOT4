namespace Hot.SMPP.Interfaces;

public interface ISMPPService
{
    public Task StartAsync();
    public Task StopAsync();
    public Task SendPendingSMSAsync();
    public Task RebindAsync();
    public bool CanSend();
}
