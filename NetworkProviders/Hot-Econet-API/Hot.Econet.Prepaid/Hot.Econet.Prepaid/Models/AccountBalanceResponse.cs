using Horizon.XmlRpc.Core;
using System.Collections;
using System.Text.Json;

namespace Hot.Econet.Prepaid.Models;

public class AccountBalanceResponse
{
    public List<AccountBalance> AccountBalances { get; set; } = new();
    public string RawResponseData { get; set; }= string.Empty;

    public static AccountBalanceResponse Get(object response)
    {
        AccountBalanceResponse rpcResponse = new();
        if  (response is not null)
        {
            rpcResponse.AccountBalances = new List<AccountBalance>();
            foreach (XmlRpcStruct item in (IList)response)
            { 
                rpcResponse.AccountBalances.Add(
                    new AccountBalance()
                    {
                        AccountType = (int)item["AccountType"],
                        Amount = Convert.ToInt64(item["Amount"]),
                        Currency = (int)item["Currency"]
                    });
            }
        }
        rpcResponse.RawResponseData += JsonSerializer.Serialize(response);
        return rpcResponse;
    }
}
 