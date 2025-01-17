using Hot4.Core.Helper;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class WebRequestRepository : RepositoryBase<WebRequests>, IWebRequestRepository
    {
        public WebRequestRepository(HotDbContext context) : base(context) { }
        public async Task<bool> AddWebRequest(WebRequests webRequests)
        {
            await Create(webRequests);
            await SaveChanges();
            return true;
        }

        public async Task<bool> DeleteWebRequest(WebRequests webRequests)
        {
            Delete(webRequests);
            await SaveChanges();
            return true;
        }

        public async Task<WebRequests?> GetWebRequestById(long webId)
        {
            return await GetById(webId);
        }

        public async Task<List<WebRequests>> GetWebRequestByRefAndAccessId(string agentRef, long accessId)
        {
            return await GetByCondition(d => d.AgentReference == agentRef && d.AccessId == accessId)
                .OrderByDescending(d => d.WebId).ToListAsync();
        }

        public async Task<List<WebRequests>> ListWebRequest(int pageNo, int pageSize)
        {
            return await PaginationFilter.GetPagedData(GetAll().OrderByDescending(d => d.WebId), pageNo, pageNo)
                 .Queryable.ToListAsync();
        }

        public async Task<bool> UpdateWebRequest(WebRequests webRequests)
        {
            Update(webRequests);
            await SaveChanges();
            return true;
        }
    }
}
