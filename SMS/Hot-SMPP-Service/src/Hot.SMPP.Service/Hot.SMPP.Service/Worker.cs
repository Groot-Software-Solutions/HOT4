using Hot.SMPP.Interfaces;
using System.Timers;

namespace Hot.SMPP.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly ISMPPService SMPPService;
        private readonly int RebindTimer = 360000;
        private readonly int SMSCheckInterval = 3000;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, ISMPPService SMPPService)
        {
            this.logger = logger;
            this.SMPPService = SMPPService;
            RebindTimer = configuration.GetValue<int>("RebindTimer",360000);
            SMSCheckInterval = configuration.GetValue<int>("SMSCheckInterval", 3000);


        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {

                await SMPPService.StartAsync();
                DateTimeOffset LastRebind = DateTimeOffset.Now;
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (SMPPService.CanSend()) await SMPPService.SendPendingSMSAsync();
                    if (LastRebind.AddSeconds(RebindTimer) < DateTimeOffset.Now)
                    {
                        await RebindAsync();
                        LastRebind = DateTimeOffset.Now;
                    }
                    await Task.Delay(SMSCheckInterval, stoppingToken);

                }
                await SMPPService.StopAsync();
            }
            catch (TaskCanceledException) { }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Critical Error Occured: {message}", ex.Message);
                Environment.Exit(1);
            }
        }


        private async Task RebindAsync()
        {
            logger.LogInformation("Rebinding SMPPs at: {time}", DateTimeOffset.Now);
            await SMPPService.RebindAsync();
        }
    }
}