using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Helpers;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using Hot.Ecocash.Application.Common;
using Hot.Ecocash.Application.Common.Interfaces;
using Hot.Ecocash.Application.Common.Models;
using Hot.Ecocash.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hot.Ecocash.Application.Actions
{
    public class FundWalletCommand : IRequest<OneOf<EcocashResult, AppException>>
    {
        public FundWalletCommand(FundWalletRequest fundWallet)
        {
            FundWallet = fundWallet;
        }

        public FundWalletRequest FundWallet { get; set; }



        public class FundWalletCommandHandler : IRequestHandler<FundWalletCommand, OneOf<EcocashResult, AppException>>
        {
            private readonly IDbContext context;
            private readonly ILogger<FundWalletCommandHandler> logger;
            private readonly IEcocashServiceFactory ecocash;

            public FundWalletCommandHandler
                (IDbContext context, ILogger<FundWalletCommandHandler> logger, IEcocashServiceFactory ecocash)
            {
                this.context = context;
                this.logger = logger;
                this.ecocash = ecocash;
            }

            public async Task<OneOf<EcocashResult, AppException>> Handle(FundWalletCommand request, CancellationToken cancellationToken)
            {
                var accessResponse = await context.Accesss.SelectCodeAsync(request.FundWallet.AccessCode);
                if (accessResponse.IsT1) return CommandHelper.ReturnDbException(accessResponse.AsT1, logger);
                var access = accessResponse.AsT0;

                OneOf<BankTrxBatch, HotDbException> bankTrxBatchResponse = await GetBankTrxBatch();
                if (bankTrxBatchResponse.IsT1) return CommandHelper.ReturnDbException(bankTrxBatchResponse.AsT1, logger);
                var bankTrxBatch = bankTrxBatchResponse.AsT0;

                BankTrx bankTrx = CreateBankTrx(request, access, bankTrxBatch);
                var banktrxResponse = await context.BankTrxs.AddAsync(bankTrx);
                if (banktrxResponse.IsT1) return CommandHelper.ReturnDbException(banktrxResponse.AsT1, logger);
                bankTrx.BankTrxID = banktrxResponse.AsT0;

                EcocashAccounts account = (EcocashAccounts)request.FundWallet.EcocashAccount;
                EcocashResult ecocashResult = await SendEcocashRequest(request, bankTrx, account);

                bankTrx.BankTrxStateID = (byte)(ecocashResult.ValidResponse ? 6 : 5);
                var bankTrxUpdate = await context.BankTrxs.UpdateAsync(bankTrx);

                return ecocashResult;
            }

            private async Task<EcocashResult> SendEcocashRequest(FundWalletCommand request, BankTrx bankTrx, EcocashAccounts account)
            {
                var ecocashService = ecocash.GetService(account);
                var ecocashResult = await ecocashService.ChargeNumberAsync
                    (request.FundWallet.TargetMobile, bankTrx.BankTrxID.ToString(), request.FundWallet.Amount, request.FundWallet.OnBehalfOf);
                bankTrx.BankRef = ecocashResult.ValidResponse
                    ? ecocashResult.Item.ecocashReference
                    : ecocashResult.ErrorData;
                return ecocashResult;
            }

            private static BankTrx CreateBankTrx(FundWalletCommand request, Access access, BankTrxBatch bankTrxBatch)
            {
                return new BankTrx()
                {
                    BankTrxBatchID = bankTrxBatch.BankTrxBatchID,
                    Amount = request.FundWallet.Amount,
                    TrxDate = DateTime.Now,
                    RefName = request.FundWallet.ReferenceId,
                    Identifier = request.FundWallet.TargetMobile,
                    Branch = $"API-{access.AccessId}",
                    BankRef = "pending",
                    Balance = 0,
                    BankTrxStateID = 0,
                    BankTrxTypeID = 17,

                };
            }

            private async Task<OneOf<BankTrxBatch, HotDbException>> GetBankTrxBatch()
            {
                return await context.BankTrxBatches.GetCurrentBatchAsync(
                    new BankTrxBatch()
                    {
                        BankID = 6,
                        LastUser = "SMSUser",
                        BatchDate = DateTime.Now,
                        BatchReference = $"SMSMerchant {DateTime.Now:dd mmm yyyy}"
                    });
            }
        }
    }
}
