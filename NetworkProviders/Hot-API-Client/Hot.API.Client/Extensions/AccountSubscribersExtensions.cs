using Hot.API.Client.Common;
using Hot.API.Client.Models;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Client
{
    public static class AccountSubscribersExtensions
    {
        public static async Task<OneOf<SubscribersGetResponse, APIException,string>> SubscribersGetListAsync(this HotAPIClient hotAPIClient)
        {
            return await hotAPIClient.apiService.Get<SubscribersGetResponse>("dealers/subscribers-get-list");
        }
        public static OneOf<SubscribersGetResponse, APIException,string> SubscribersGetList(this HotAPIClient hotAPIClient)
        {
            return SubscribersGetListAsync(hotAPIClient).Result;
        }

        public static async Task<OneOf<SubscriberDeleteResponse, APIException,string>> SubscribersDeleteAsync(this HotAPIClient hotAPIClient,long SubscriberId)
        {
            return await hotAPIClient.apiService.Get<SubscriberDeleteResponse>($"subscribers-delete?SubscriberId={SubscriberId}");
        }
        public static OneOf<SubscriberDeleteResponse, APIException,string> SubscribersDelete(this HotAPIClient hotAPIClient,long SubscriberId)
        {
            return SubscribersDeleteAsync(hotAPIClient, SubscriberId).Result;
        }

        public static async Task<OneOf<SubscriberSaveResponse, APIException,string>> SubscribersSaveAsync(this HotAPIClient hotAPIClient,string Name, string Mobile, long SubscriberId)
        {
            var parameters = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Name", Name),
                    new KeyValuePair<string, string>("Mobile", Mobile),
                    new KeyValuePair<string, string>("SubscriberId", SubscriberId.ToString() )
                });
            return await hotAPIClient.apiService.Post<SubscriberSaveResponse>("agents/subscribers-save", parameters);

        }
        public static OneOf<SubscriberSaveResponse, APIException,string> SubscribersSave(this HotAPIClient hotAPIClient,string Name, string Mobile, long SubscriberId)
        {
            return SubscribersSaveAsync(hotAPIClient, Name, Mobile, SubscriberId).GetAwaiter().GetResult();
        }

    }
}
