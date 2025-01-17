using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IWebRequestRepository
    {
        Task<List<WebRequests>> ListWebRequest(int pageNo, int pageSize);
        Task<bool> AddWebRequest(WebRequests webRequest);
        Task<bool> UpdateWebRequest(WebRequests webRequest);
        Task<bool> DeleteWebRequest(WebRequests webRequest);
        Task<WebRequests?> GetWebRequestById(long webId);
        Task<List<WebRequests>> GetWebRequestByRefAndAccessId(string agentRef, long accessId);
    }
}
