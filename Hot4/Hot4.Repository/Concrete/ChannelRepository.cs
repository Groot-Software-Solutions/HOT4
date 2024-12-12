using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class ChannelRepository : RepositoryBase<TblChannel>, IChannelRepository
    {
        public ChannelRepository(HotDbContext context) : base(context) { }
        public async Task<List<TblChannel>> GetChannels()
        {
            return await GetAll().ToListAsync();
        }
    }
}
