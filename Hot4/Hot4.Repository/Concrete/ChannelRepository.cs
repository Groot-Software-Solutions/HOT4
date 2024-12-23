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
        public async Task<List<ChannelModel>> GetChannels()
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
