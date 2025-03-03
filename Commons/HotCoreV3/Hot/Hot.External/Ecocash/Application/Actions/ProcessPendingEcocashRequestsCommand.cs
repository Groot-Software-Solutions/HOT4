using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Ecocash.Application.Common.Extensions;
using Hot.Ecocash.Application.Common.Interfaces;
using Hot.Ecocash.Domain.Entities;
using Hot.Ecocash.Domain.Enums;
using MediatR;
using OneOf;

namespace Hot.Ecocash.Application.Actions;
public record ProcessPendingEcocashRequestsCommand : IRequest<OneOf<List<Transaction>, AppException>>;
public class ProcessPendingEcocashRequestsCommandHandler : IRequestHandler<ProcessPendingEcocashRequestsCommand, OneOf<List<Transaction>, AppException>>
{
    private readonly IDbContext context;
    private readonly IEcocashServiceFactory serviceFactory;

    public ProcessPendingEcocashRequestsCommandHandler(IDbContext context, IEcocashServiceFactory serviceFactory)
    {
        this.context = context;
        this.serviceFactory = serviceFactory;
    }

    public async Task<OneOf<List<Transaction>, AppException>> Handle(ProcessPendingEcocashRequestsCommand request, CancellationToken cancellationToken)
    {
        var paymentsListResult = await context.BankTrxs.ListNewEcocashAsync();
        if (paymentsListResult.IsT1) return new AppException("Failed to get pending payments", paymentsListResult.AsT1);
        var paymentsList = paymentsListResult.AsT0;
        List<Transaction> results = new();
        foreach (var item in paymentsList)
        {
            var account = item.GetEcocashAccountType();
            var currency = GetCurrency(account);
            var service = serviceFactory.GetService(account);
            var result = await service.ChargeNumberAsync(item.Identifier, item.BankTrxID.ToString(), item.Amount, "CommShop", "Airtime", currency);

            item.BankRef = result.ValidResponse ? result.Item.ecocashReference ?? result.Item.transactionOperationStatus ?? "" : result.ErrorData ?? "";
            item.BankTrxStateID = (byte)(result.ValidResponse ? 6 : 5);
            _ = await context.BankTrxs.UpdateAsync(item);
            if (result.ValidResponse) results.Add(result.Item);

        }
        return results;
    }

    private static Currencies GetCurrency(EcocashAccounts account)
    {
        return account switch {
            EcocashAccounts.FCAAccount => Currencies.USD_FCA, 
            _ => Currencies.ZiG
        };
    }
}

