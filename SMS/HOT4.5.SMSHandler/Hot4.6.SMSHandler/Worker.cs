using Hot.Application.Actions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hot.SMSHandler
{
    public class Worker : BackgroundService
    {
        private readonly int CheckSMSInterval;
        private readonly ILogger<Worker> _logger;
        private readonly IMediator mediator;

        public Worker(ILogger<Worker> logger, IMediator mediator, IConfiguration configuration)
        {
            CheckSMSInterval = configuration.GetValue<int>("CheckSMSInterval");
            _logger = logger;
            this.mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await mediator
                   .Send(new HandlePendingSMSCommand());
                _logger.LogInformation($"SMS Handler services checked messages at: {DateTime.Now:dd MMM yy - HH:mm:ss}", DateTimeOffset.Now);
                await Task.Delay(CheckSMSInterval, stoppingToken);
            }
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
