using Horizon.XmlRpc.Client;

namespace Hot.Econet.Prepaid.Interfaces.APIEndpoints;

public interface IEconetServiceProxy : IXmlRpcProxy, IAccountBalanceEnquiry, IPurchaseAirtime, IPurchaseBundle, IValidateMobile
{
}