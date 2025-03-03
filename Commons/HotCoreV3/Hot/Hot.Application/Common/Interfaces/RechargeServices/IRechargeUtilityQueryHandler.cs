namespace Hot.Application.Common.Interfaces;

public interface IRechargeUtilityQueryHandler
{
    public int NetworkId { get; set; }

    public Task<OneOf<UtilityAccountDetailsModel, NotFoundException, NetworkProviderException, AppException>> AccountDetails(string AccountNumber);

}
