namespace Hot.Application.Common.Models;
public class StatementTransactionModel : StatementTransaction
{
    public decimal Balance { get; set; }
}
public class TransactionModelProfile : AutoMapper.Profile
{
    public TransactionModelProfile()
    {
        CreateMap<StatementTransaction, StatementTransactionModel>().ReverseMap();
    }
}
