using Hot.Ecocash.Application.Common;
using Hot.Ecocash.Lesotho.Models;
using Microsoft.Extensions.Options;
using Refit;
using System.Net.Http.Headers;

namespace Hot.Ecocash.Lesotho.Services;

public interface IEcocashLesothoIdentityAPI
{
    [Headers("Content-Type: application/x-www-form-urlencoded")]
    [Get("/token")] 
    public Task<TokenResultLesotho> GetToken([Body(BodySerializationMethod.UrlEncoded)] FormUrlEncodedContent request);
}
public interface IEcocashLesothoAPI
{
    [Post("/openapi/PayMerchant")]
    public Task<PayMerchantResponseLesothoModel> PayMerchant(PayMerchantRequestLesothoModel request, [Authorize] string token);

    [Post("/openapi/QueryTransactionStatus")]
    public Task<QueryTransactionResponseLesothoModel> QueryTransaction(QueryTransactionRequestLesothoModel request, [Authorize] string token);
}

//public class EcocashLesothoAuthHandler : DelegatingHandler
//{
//    public readonly IEcocashLesothoIdentityAPI ApiService;
//    private string Token { get; set; } = string.Empty; 
//    private ServiceOptions options;

//    public EcocashLesothoAuthHandler(IEcocashLesothoIdentityAPI apiService)
//    {
//        ApiService = apiService; 
//    }
//    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//    {
//        if (!IsValidToken(Token)) await UpdateTokenAsync();

//        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);

//        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
//    }

//    private async Task UpdateTokenAsync()
//    {
//        var response = await ApiService.GetToken(GetAuthRequest());
//        Token = response.access_token;
//    }

//    private bool IsValidToken(string token)
//    {
//        if (string.IsNullOrEmpty(token)) return false;

//        return true;
//    }
//    private FormUrlEncodedContent GetAuthRequest()
//    {
//        return new(new List<KeyValuePair<string, string>>
//{
//            new KeyValuePair<string, string>("grant_type" , "password"),
//            new KeyValuePair<string, string>("password" , options.Password),
//            new KeyValuePair<string, string>("username" , options.Username),
//        });
//    }
//}


