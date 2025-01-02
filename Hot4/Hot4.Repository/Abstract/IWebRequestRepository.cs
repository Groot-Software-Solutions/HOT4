using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IWebRequestRepository
    {
        Task<List<WebRequestModel>> ListWebRequest(int pageNo, int pageSize);
        Task AddWebRequest(WebRequests webRequest);
        Task UpdateWebRequest(WebRequests webRequest);
        Task DeleteWebRequest(WebRequests webRequest);
        Task<WebRequestModel?> GetWebRequestById(long webId);
        Task<List<WebRequestModel>> GetWebRequestByRefAndAccessId(string agentRef, long accessId);
    }
}
