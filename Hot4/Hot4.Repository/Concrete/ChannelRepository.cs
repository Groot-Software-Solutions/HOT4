using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel.ApiModels;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class ChannelRepository : RepositoryBase<Channels>, IChannelRepository
    {
        public ChannelRepository(HotDbContext context) : base(context) { }

        public async Task AddChannel(Channels channel)
        {
            await Create(channel);
            await SaveChanges();
        }

        public async Task DeleteChannel(Channels channel)
        {
            await Delete(channel);
            await SaveChanges();
        }
        public async Task UpdateChannel(Channels channel)
        {
            await Update(channel);
            await SaveChanges();
        }
        public async Task<List<ChannelModel>> GetChannel()
        {
            return await GetAll()
                .Select(d => new ChannelModel
                {
                    ChannelId = d.ChannelId,
                    Channel = d.Channel
                }).OrderBy(d => d.ChannelId)
                .ToListAsync();
        }


    }
}
