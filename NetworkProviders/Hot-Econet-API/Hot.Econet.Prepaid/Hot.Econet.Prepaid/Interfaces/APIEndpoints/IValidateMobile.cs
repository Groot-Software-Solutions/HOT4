using Horizon.XmlRpc.Core;

namespace Hot.Econet.Prepaid.Interfaces.APIEndpoints;

public interface IValidateMobile
{
    [XmlRpcMethod("validate_msisdn")]
    object validate_mobile(
          string username
        , string password
        , LoadDataRequest loadDataRequest
        );
}
