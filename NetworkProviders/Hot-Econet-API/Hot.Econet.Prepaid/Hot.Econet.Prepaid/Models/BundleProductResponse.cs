using Horizon.XmlRpc.Core;
using System.Collections;
using System.Text.Json;

namespace Hot.Econet.Prepaid.Models;

public class BundleProductResponse
{
    public List<BundleProductItem> Bundles = new();
    public string RawResponseData = string.Empty;

    public static BundleProductResponse Get(XmlRpcResponse response)
    {
        BundleProductResponse rpcResponse = new();
        if (response.retVal is not null)
        {
            rpcResponse.Bundles = new List<BundleProductItem>();
            foreach (Hashtable item in (ArrayList)response.retVal)
            {
                rpcResponse.Bundles.Add(
                    new BundleProductItem()
                    {
                        ProviderCode = (int)(item["ProviderCode"] ?? 0),
                        ProductCode = (string)(item["ProductCode"] ?? ""),
                        Currency = (int)(item["Currency"] ?? 0),
                        Quantity = (int)(item["Quantity"] ?? 0),
                        Amount = (int)(item["Amount"] ?? 0)
                    });
            }
        }
        rpcResponse.RawResponseData += JsonSerializer.Serialize(response);
        return rpcResponse;
    }
}

