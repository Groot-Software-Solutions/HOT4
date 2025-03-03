 
using System.Text.Json;

namespace Hot.Econet.Prepaid.Models;

public class LoadDataResponse : DataResponse
{ 
    public static LoadDataResponse Get(object response)
    {
        LoadDataResponse rpcResponse = new();
        if (response is not null)
        {
            var responseObject = (System.Collections.Hashtable)response;
            rpcResponse.Description = (string)(responseObject["Description"] ?? "");
            rpcResponse.ProviderReference = (string)(responseObject["ProviderReference"] ?? "");
            rpcResponse.Serial = (string)(responseObject["Serial"] ?? "");
            rpcResponse.Status = (int)(responseObject["Status"] ?? "");
            rpcResponse.StatusCode = (int)(responseObject["StatusCode"] ?? "");
        }
        rpcResponse.RawResponseData += JsonSerializer.Serialize(response);
        return rpcResponse;
    } 
}






