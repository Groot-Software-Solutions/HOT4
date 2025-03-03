namespace Hot.Application.Actions;
public record HandlePendingSMSCommand : IRequest<Unit>;
        public class CheckPendingSMSCommandHandler : IRequestHandler<HandlePendingSMSCommand, Unit>
        {
            private readonly IDbContext dbcontext;
            private readonly ILogger<CheckPendingSMSCommandHandler> logger;
            private readonly IMediator mediator;

            public CheckPendingSMSCommandHandler(IDbContext dbcontext, ILogger<CheckPendingSMSCommandHandler> logger, IMediator mediator)
            {
                this.dbcontext = dbcontext;
                this.logger = logger;
                this.mediator = mediator;
            }

            public async Task<Unit> Handle(HandlePendingSMSCommand request, CancellationToken cancellationToken)
            {
                List<SMS> pendingSMSs = new List<SMS>();

                var getSMSsResponse = await dbcontext.SMSs.InboxAsync();
                getSMSsResponse.Switch(
                    list => pendingSMSs.AddRange(list),
                    error => LogException("GetSMSs", error)
                    );
                var dealers = pendingSMSs.Select(s => s.Mobile).Distinct();

                Parallel.ForEach(
                    dealers,
                    (dealer) =>
                    {
                        pendingSMSs
                            .Where(s => s.Mobile == dealer)
                            .ToList()
                            .ForEach(async sms => 
                                await mediator.Send(new HandleSMSCommand(sms))
                                );
                    });

                LogDebugInformation( "Handle",
                     $"Completed Handling Pending sms Count: {pendingSMSs.Count()} Date: {DateTime.Now:dd-MMM-yy HH:mm:ss}");

                return Unit.Value;
            } 
           
            private void LogDebugInformation( string logMethod, string message)
            {
                logger.LogDebug($"SMS Handler Service: HandlePending - {logMethod}{message}");
            }

            private void LogException( string logMethod, Exception exception)
            {
                logger.LogError(exception, $"SMS Handler Service: HandlePending - { logMethod}");
            }
        }
