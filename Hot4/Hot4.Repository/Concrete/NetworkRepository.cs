﻿using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.ViewModel.ApiModels;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class NetworkRepository : RepositoryBase<Networks>, INetworkRepository
    {
        public NetworkRepository(HotDbContext context) : base(context) { }
        public async Task<List<NetworkModel>> GetNetworkIdentity(string mobile)
        {
            return await GetByCondition(d => mobile.Substring(1, 2) == d.Prefix || mobile.Substring(1, 4) == d.Prefix)
                 .Select(d => new NetworkModel
                 {
                     NetworkId = d.NetworkId,
                     Network = d.Network,
                     Prefix = d.Prefix
                 }).ToListAsync();
        }
    }
}
