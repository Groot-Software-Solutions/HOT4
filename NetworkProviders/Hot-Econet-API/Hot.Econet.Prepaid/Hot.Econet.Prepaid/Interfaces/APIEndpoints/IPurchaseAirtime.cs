using Horizon.XmlRpc.Core;

namespace Hot.Econet.Prepaid.Interfaces.APIEndpoints;

public interface IPurchaseAirtime
{
    [XmlRpcMethod("load_value")]
    object purchase_airtime(
        string username,
        string password,
        LoadAirtimeRequest loadAirtimeRequest
    );
}
