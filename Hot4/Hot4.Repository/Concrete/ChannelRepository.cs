using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class ChannelRepository : RepositoryBase<Channels>, IChannelRepository
    {
        public ChannelRepository(HotDbContext context) : base(context) { }
        public async Task<bool> AddChannel(Channels channel)
        {
            await Create(channel);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteChannel(Channels channel)
        {
            Delete(channel);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdateChannel(Channels channel)
        {
            Update(channel);
            await SaveChanges();
            return true;
        }     
        public async Task<List<Channels>> ListChannel()
        {
            var records = await GetAll()
                .OrderBy(d => d.ChannelId)
                .ToListAsync();
            return records;  
        }
        public async Task<Channels> GetByChannelId(byte channelId)
        {
            var record = await GetById(channelId);
            return record;
        }
    }
}
