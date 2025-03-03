using Horizon.XmlRpc.Core;

namespace Hot.Econet.Prepaid.Models;

public class GetTransactionStatusResponse : DataResponse
{
    public static GetTransactionStatusResponse Get(XmlRpcResponse response)
    {
        GetTransactionStatusResponse rpcResponse = new();
        if (response.retVal is not null)
        {
            var responseObject = (System.Collections.Hashtable)response.retVal;
            rpcResponse.Description = (string)(responseObject["Description"] ?? "");
            rpcResponse.ProviderReference = (string)(responseObject["ProviderReference"] ?? "");
            rpcResponse.Serial = (string)(responseObject["Serial"] ?? "");
            rpcResponse.Status = (int)(responseObject["Status"] ?? "");
            rpcResponse.StatusCode = (int)(responseObject["StatusCode"] ?? "");
        }
        rpcResponse.RawResponseData += response;
        return rpcResponse;
    }
}
 