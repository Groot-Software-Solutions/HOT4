using Horizon.XmlRpc.Core;
using System.Text.Json;

namespace Hot.Econet.Prepaid.Models;

public class LoadAirtimeResponse : DataResponse
{ 
    public static LoadAirtimeResponse Get(object response)
    {
        LoadAirtimeResponse rpcResponse = new LoadAirtimeResponse();
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
