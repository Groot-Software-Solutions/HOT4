using AutoMapper;

namespace Hot.Application.Actions.AccountActions;
public record GetAccountByAccountIdQuery(long AccountId) : IRequest<OneOf<AccountDetailedModel, NotFoundException, AppException>>;

        public class GetAccountByAccountIdQueryHandler : IRequestHandler<GetAccountByAccountIdQuery, OneOf<AccountDetailedModel, NotFoundException, AppException>>
        {
            private readonly IDbContext _context;
            private readonly IMapper _mapper;
            private readonly ILogger<GetAccountByAccountIdQueryHandler> _logger;

            public GetAccountByAccountIdQueryHandler(IDbContext context, IMapper mapper, ILogger<GetAccountByAccountIdQueryHandler> logger)
            {
                _context = context;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<OneOf<AccountDetailedModel, NotFoundException, AppException>> Handle(GetAccountByAccountIdQuery request, CancellationToken cancellationToken)
            {
                var accountResponse = await _context.Accounts.GetAsync((int)request.AccountId);
                if (accountResponse.ResultOrNull() == null)
                {
                    var error = accountResponse.AsT1;
                    if (error.IsNotFoundException()) return accountResponse.AsT1.ReturnNotFound( accountResponse.AsT1.Message, "");
                    return new NotFoundException(error.Message, "");
                }
                var account = accountResponse.AsT0;
                var accountModel = _mapper.Map<AccountDetailedModel>(account);

                var profiles = (await _context.Profiles.ListAsync()).ResultOrNull();
                accountModel.ProfileName = profiles is null ? "" :
                    profiles.FirstOrDefault(p => p.ProfileId == account.ProfileID)?.ProfileName ?? "";

                var accessResponse = await _context.Accesss.ListAsync(request.AccountId);
        if (accessResponse.ResultOrNull() == null) return accessResponse.AsT1.LogAndReturnError(accessResponse.AsT1.Message, _logger);
                var accesses = accessResponse.ResultOrNull() ?? new List<Access>();
                accountModel.Accesses = accesses;
                accountModel.AccessWebs = new();
                accesses.ForEach(async a =>
                {
                    var accessWebResult = (await _context.AccessWebs.GetAsync((int)a.AccessId)).ResultOrNull();
                    if (accessWebResult is not null) accountModel.AccessWebs.Add(accessWebResult);
                });
                 
                var addressResponse = await _context.Addresses.GetAsync((int)request.AccountId);
        if (addressResponse.ResultOrNull() == null) return addressResponse.AsT1.LogAndReturnError(addressResponse.AsT1.Message,_logger);
                var address = addressResponse.ResultOrNull() ?? new Address();
                accountModel.Address = address; //Query for address


                return accountModel;
            }

        }