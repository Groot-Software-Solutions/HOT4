using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class ConfigRepository : RepositoryBase<Configs>, IConfigRepository
    {
        public ConfigRepository(HotDbContext context) : base(context) { }
        public async Task<Configs?> GetConfig()
        {
            return await GetAll().FirstOrDefaultAsync();
        }
    }
}
