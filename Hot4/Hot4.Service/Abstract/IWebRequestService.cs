using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IWebRequestService
    {
        Task<List<WebRequestModel>> ListWebRequest(int pageNo, int pageSize);
        Task<bool> AddWebRequest(WebRequestModel webRequest);
        Task<bool> UpdateWebRequest(WebRequestModel webRequest);
        Task<bool> DeleteWebRequest(long webId);
        Task<WebRequestModel?> GetWebRequestById(long webId);
        Task<List<WebRequestModel>> GetWebRequestByRefAndAccessId(string agentRef, long accessId);
    }
}
