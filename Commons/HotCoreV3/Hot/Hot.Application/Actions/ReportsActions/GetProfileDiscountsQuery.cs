namespace Hot.Application.Actions.ReportsActions
{

    public record GetProfileDiscountsQuery(int ReportId, int? WalletId) : IRequest<OneOf<List<ProfileDiscountResult>, AppException>>;
    public class GetProfileDiscountsQueryHandler : IRequestHandler<GetProfileDiscountsQuery, OneOf<List<ProfileDiscountResult>, AppException>>
    {
        private readonly IDbContext context;
        private readonly ILogger<GetProfileDiscountsQueryHandler> logger;

        public GetProfileDiscountsQueryHandler(IDbContext context, ILogger<GetProfileDiscountsQueryHandler> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<OneOf<List<ProfileDiscountResult>, AppException>> Handle(GetProfileDiscountsQuery request, CancellationToken cancellationToken)
        {
            var response = await context.Report.GetProfileDiscountsAsync(request.ReportId, request.WalletId);
            if (response.IsT1) return response.AsT1.LogAndReturnError(logger);
            var discountslist = response.AsT0;

            return discountslist;
        }

    }
}
