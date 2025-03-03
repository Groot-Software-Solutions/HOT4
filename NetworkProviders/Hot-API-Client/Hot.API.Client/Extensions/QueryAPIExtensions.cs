using Hot.API.Client.Common;
using Hot.API.Client.Models;
using OneOf;
using System.Threading.Tasks;

namespace Hot.API.Client
{
    public static class QueryAPIExtensions
    {
        public static OneOf<QueryTransactionResponse, APIException, string> QueryTransaction
            (this HotAPIClient hotAPIClient, string AgentReference)
        {
            return QueryTransactionAsync(hotAPIClient, AgentReference).Result;
        }

        public static async Task<OneOf<QueryTransactionResponse, APIException, string>> QueryTransactionAsync
            (this HotAPIClient hotAPIClient, string AgentReference)
        {
            return await hotAPIClient.apiService.Get<QueryTransactionResponse>(
                "agents/query-transaction?agentReference=" + AgentReference);
        }


        public static OneOf<TransactionsGetResponse, APIException, string> TransactionsGetRecent
            (this HotAPIClient hotAPIClient)
        {
            return TransactionsGetRecentAsync(hotAPIClient).Result;
        }

        public static async Task<OneOf<TransactionsGetResponse, APIException, string>> TransactionsGetRecentAsync
            (this HotAPIClient hotAPIClient)
        {
            return await hotAPIClient.apiService.Get<TransactionsGetResponse>("dealers/transactions-query-recent");
        }


    }
}
