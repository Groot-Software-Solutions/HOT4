namespace Hot.Application.Actions.AccountActions;

public record ChangePasswordCommand(string AccessCode, string AccessPassword, string NewPassword) : IRequest<OneOf<bool, AppException>>;
        public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, OneOf<bool, AppException>>
        {
            private readonly IDbContext context;
            private readonly ILogger<ChangePasswordCommandHandler> logger;

            public ChangePasswordCommandHandler(IDbContext context, ILogger<ChangePasswordCommandHandler> logger)
            {
                this.context = context;
                this.logger = logger;
            }

            public async Task<OneOf<bool, AppException>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
            {
                var accessResult = await context.Accesss.SelectLoginAsync(request.AccessCode,request.AccessPassword);
                var access = accessResult.ResultOrNull();
                if (access is null) return accessResult.AsT1.LogAndReturnError("Invalid Account or Password.",logger);
                access.AccessPassword = request.NewPassword;
                var result = (await context.Accesss.PasswordChangeAsync(access)).ResultOrNull();
                return result;
            }
        }

