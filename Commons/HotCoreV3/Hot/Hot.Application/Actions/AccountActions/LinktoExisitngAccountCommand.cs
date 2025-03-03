namespace Hot.Application.Actions.AccountActions;
public class LinktoExisitngAccountCommand : IRequest<OneOf<LinkAccountResult, AppException>>
    {
        public LinktoExisitngAccountCommand(long accountId, string accessCodeToLink, string newAccessPassword)
        {
            AccountId = accountId;
            AccessCodeToLink = accessCodeToLink;
            NewAccessPassword = newAccessPassword;
        }

        public long AccountId { get; set; }
        public string AccessCodeToLink { get; set; }
        public string NewAccessPassword { get; set; }

        public class LinktoExistingAccountCommandHandler : IRequestHandler<LinktoExisitngAccountCommand, OneOf<LinkAccountResult, AppException>>
        {
            private readonly IDbContext context;
            private readonly ILogger<LinktoExistingAccountCommandHandler> logger;

            public LinktoExistingAccountCommandHandler(IDbContext context, ILogger<LinktoExistingAccountCommandHandler> logger)
            {
                this.context = context;
                this.logger = logger;
            }

            public async Task<OneOf<LinkAccountResult, AppException>> Handle(LinktoExisitngAccountCommand request, CancellationToken cancellationToken)
            {
                NormalizeAccessCode(request);
                var exists = (await context.Accesss.SelectCodeAsync(request.AccessCodeToLink)).ResultOrNull();
                if (!(exists is null)) return new AppException("Account already exists.", logger);

                var newaccess = new Access()
                {
                    AccessCode = request.AccessCodeToLink,
                    AccessPassword = request.NewAccessPassword,
                    AccountId = request.AccountId,
                    ChannelID = (byte)(request.AccessCodeToLink.StartsWith("07") ? Channels.Sms : Channels.Web),
                };

                var result = await context.Accesss.AddAsync(newaccess);
                if (result.ResultOrNull() == -1) return result.AsT1.LogAndReturnError("Link AccessCode to Account Error", logger);
                newaccess.AccessId = result.ResultOrNull();
                var account = (await context.Accounts.GetAsync((int)request.AccountId)).ResultOrNull();

                return new LinkAccountResult()
                {
                    AccountId = request.AccountId,
                    AccountName = account.AccountName,
                    Success = true,
                    AccessCode = request.AccessCodeToLink
                };
            }

            private static void NormalizeAccessCode(LinktoExisitngAccountCommand request)
            {
                request.AccessCodeToLink = request.AccessCodeToLink.Trim().ToLower();
            }

        }
    }
