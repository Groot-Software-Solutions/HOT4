using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Helpers;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using Hot.Ecocash.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf; 

namespace Hot.Ecocash.Application.Actions
{
    public class AddNewNetOneHotDealsPromoSelfTopUpCommand : IRequest<OneOf<bool, AppException>>
    {
        public AddNewNetOneHotDealsPromoSelfTopUpCommand(string targetMobile, string accessCode, decimal amount)
        {
            TargetMobile = targetMobile;
            AccessCode = accessCode;
            Amount = amount;
        }

        public string TargetMobile { get; set; }
        public string AccessCode { get; set; }
        public decimal Amount { get; set; }

        public class AddNewNetOneHotDealsPromoSelfTopUpCommandHandler : IRequestHandler<AddNewNetOneHotDealsPromoSelfTopUpCommand, OneOf<bool, AppException>>
        {
            private readonly IDbContext context;
            private readonly ILogger<AddNewNetOneHotDealsPromoSelfTopUpCommandHandler> logger;
            private readonly IEcocashServiceFactory ecocash;

            public AddNewNetOneHotDealsPromoSelfTopUpCommandHandler
                (IDbContext context, ILogger<AddNewNetOneHotDealsPromoSelfTopUpCommandHandler> logger, IEcocashServiceFactory ecocash)
            {
                this.context = context;
                this.logger = logger;
                this.ecocash = ecocash;
            }

            public async Task<OneOf<bool, AppException>> Handle(AddNewNetOneHotDealsPromoSelfTopUpCommand request, CancellationToken cancellationToken)
            {
                var accessResponse = await context.Accesss.SelectCodeAsync(request.AccessCode);
                if (accessResponse.IsT1) return CommandHelper.ReturnDbException(accessResponse.AsT1, logger);
                var access = accessResponse.AsT0;

                var bankTrxBatchResponse = await context.BankTrxBatches.GetCurrentBatchAsync(
                    new BankTrxBatch()
                    {
                        BankID = 6,
                        LastUser = "SMSUser",
                        BatchDate = DateTime.Now,
                        BatchReference = $"SMSMerchant {DateTime.Now:dd mmm yyyy}"
                    });
                if (bankTrxBatchResponse.IsT1) return CommandHelper.ReturnDbException(bankTrxBatchResponse.AsT1, logger);
                var bankTrxBatch = bankTrxBatchResponse.AsT0;

                var bankTrx = new BankTrx()
                {
                    BankTrxBatchID = bankTrxBatch.BankTrxBatchID,
                    Amount = request.Amount,
                    TrxDate = DateTime.Now,
                    RefName = $"{access.AccessId}",
                    Identifier = request.TargetMobile,
                    Branch = $"API-{access.AccessId}-Promo-{request.Amount:###0.00}",
                    BankRef = "pending",
                    Balance = 0,
                    BankTrxStateID = 0,
                    BankTrxTypeID = 17,
                };
                var banktrxResponse = await context.BankTrxs.AddAsync(bankTrx);
                if (banktrxResponse.IsT1) return CommandHelper.ReturnDbException(banktrxResponse.AsT1, logger);
                bankTrx.BankTrxID = banktrxResponse.AsT0;

                var ecocashService = ecocash.GetService(Domain.Enums.EcocashAccounts.MainAccount);
                var ecocashResult = await ecocashService.ChargeNumberAsync(request.TargetMobile, bankTrx.BankTrxID.ToString(), request.Amount, "CommShop");
                bankTrx.BankRef = ecocashResult.ValidResponse ? ecocashResult.Item.ecocashReference : ecocashResult.ErrorData;
                bankTrx.BankTrxStateID = (byte)(ecocashResult.ValidResponse ? 6 : 5);
                var bankTrxUpdate =  await context.BankTrxs.UpdateAsync(bankTrx);
                  
                return true;
            }


        }
    }
}
