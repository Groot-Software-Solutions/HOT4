using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Actions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Service
{
    public class Worker : BackgroundService
    {

        private readonly int CheckforMailTimer;
        private readonly ILogger<Worker> _logger;
        private readonly IMediator mediator;

        public Worker(ILogger<Worker> logger, IMediator mediator, IConfiguration configuration)
        {
            CheckforMailTimer = configuration.GetValue<int>("CheckforMailTimer");
            _logger = logger;
            this.mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await mediator
                   .Send(new CheckForNewSMSs());
                _logger.LogInformation($"PBX services checked messages at: {DateTime.Now:dd MMM yy - HH:mm:ss}", DateTimeOffset.Now);
                await Task.Delay(CheckforMailTimer * 1000, stoppingToken);
            }
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
