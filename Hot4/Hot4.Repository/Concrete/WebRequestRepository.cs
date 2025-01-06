using Hot4.Core.Helper;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class WebRequestRepository : RepositoryBase<WebRequests>, IWebRequestRepository
    {
        public WebRequestRepository(HotDbContext context) : base(context) { }
        public async Task AddWebRequest(WebRequests webRequests)
        {
            await Create(webRequests);
            await SaveChanges();
        }

        public async Task DeleteWebRequest(WebRequests webRequests)
        {
            Delete(webRequests);
            await SaveChanges();
        }

        public async Task<WebRequestModel?> GetWebRequestById(long webId)
        {
            var result = await GetById(webId);
            if (result != null)
            {
                return new WebRequestModel
                {
                    AccessId = result.AccessId,
                    AgentReference = result.AgentReference,
                    Amount = result.Amount,
                    ChannelId = result.ChannelId,
                    Cost = result.Cost,
                    Discount = result.Discount,
                    HotTypeId = result.HotTypeId,
                    InsertDate = result.InsertDate,
                    RechargeId = result.RechargeId,
                    Reply = result.Reply,
                    ReplyDate = result.ReplyDate,
                    ReturnCode = result.ReturnCode,
                    StateId = result.StateId,
                    WalletBalance = result.WalletBalance,
                    WebId = result.WebId,

                };
            }
            return null;
        }

        public async Task<List<WebRequestModel>> GetWebRequestByRefAndAccessId(string agentRef, long accessId)
        {
            return await GetByCondition(d => d.AgentReference == agentRef && d.AccessId == accessId)
                .Select(d => new WebRequestModel
                {
                    AccessId = d.AccessId,
                    AgentReference = d.AgentReference,
                    Amount = d.Amount,
                    ChannelId = d.ChannelId,
                    Cost = d.Cost,
                    Discount = d.Discount,
                    HotTypeId = d.HotTypeId,
                    InsertDate = d.InsertDate,
                    RechargeId = d.RechargeId,
                    Reply = d.Reply,
                    ReplyDate = d.ReplyDate,
                    ReturnCode = d.ReturnCode,
                    StateId = d.StateId,
                    WalletBalance = d.WalletBalance,
                    WebId = d.WebId,
                }).OrderByDescending(d => d.WebId).ToListAsync();
        }

        public async Task<List<WebRequestModel>> ListWebRequest(int pageNo, int pageSize)
        {
            return await PaginationFilter.GetPagedData(GetAll(), pageNo, pageNo)
                 .Queryable.Select(d => new WebRequestModel
                 {
                     AccessId = d.AccessId,
                     AgentReference = d.AgentReference,
                     Amount = d.Amount,
                     ChannelId = d.ChannelId,
                     Cost = d.Cost,
                     Discount = d.Discount,
                     HotTypeId = d.HotTypeId,
                     InsertDate = d.InsertDate,
                     RechargeId = d.RechargeId,
                     Reply = d.Reply,
                     ReplyDate = d.ReplyDate,
                     ReturnCode = d.ReturnCode,
                     StateId = d.StateId,
                     WalletBalance = d.WalletBalance,
                     WebId = d.WebId,
                 }).OrderByDescending(d => d.WebId).ToListAsync();
        }

        public async Task UpdateWebRequest(WebRequests webRequests)
        {
            Update(webRequests);
            await SaveChanges();
        }
    }
}
