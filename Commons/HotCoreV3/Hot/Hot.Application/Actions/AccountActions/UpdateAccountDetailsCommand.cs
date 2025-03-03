namespace Hot.Application.Actions.AccountActions;
public record UpdateAccountDetailsCommand(Account Account):IRequest<OneOf<bool,NotFoundException,AppException>>;
    public class UpdateAccountDetailsCommandHandler : IRequestHandler<UpdateAccountDetailsCommand, OneOf<bool, NotFoundException, AppException>>
    {
        private readonly IDbContext _context;
        private readonly ILogger<UpdateAccountDetailsCommandHandler> logger;

        public UpdateAccountDetailsCommandHandler(IDbContext context, ILogger<UpdateAccountDetailsCommandHandler> logger)
        {
            _context = context;
            this.logger = logger;
        }

        public async Task<OneOf<bool, NotFoundException, AppException>> Handle(UpdateAccountDetailsCommand request, CancellationToken cancellationToken)
        {
            Exception error;
            var accountResponse = await _context.Accounts.GetAsync((int)request.Account.AccountID);
            if (accountResponse.ResultOrNull() != null)
            {
                var updateresult = await _context.Accounts.UpdateAsync(request.Account);
                if (updateresult.IsT0) return updateresult.AsT0;
                error = updateresult.AsT1;
            }
            else
            {
                if (accountResponse.IsT0)
                {
                    return new NotFoundException($"Account not found", request.Account.AccountID);
                }
                else
                {
                    error = accountResponse.AsT1;
                }
            }
            error.LogError(logger);
            return new AppException($"{error.GetType().Name} Error", error);
        }
    }
