namespace Hot.Application.Common.Interfaces;

public interface IRechargeMobileQueryHandler
{
    public int NetworkId { get; set; }

    public Task<OneOf<MobileAccountDetailsModel, NotFoundException, NetworkProviderException, AppException>> AccountDetails(string MobileNumber);

}