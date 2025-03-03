using Horizon.XmlRpc.Core;

namespace Hot.Econet.Prepaid.Interfaces.APIEndpoints;

public interface IPurchaseBundle
{
    [XmlRpcMethod("load_bundle")]
    object purchase_bundle(
          string username
        , string password
        , LoadDataRequest loadDataRequest
        );
}
