using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class ConfigRepository : RepositoryBase<Configs>, IConfigRepository
    {
        public ConfigRepository(HotDbContext context) : base(context) { }
        public async Task<List<Configs>> ListConfig()
        {
            return await GetAll()
                .ToListAsync();
        }
        public async Task<bool> AddConfig(Configs config)
        {
            await Create(config);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteConfig(Configs config)
        {
            Delete(config);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdateConfig(Configs config)
        {
            Update(config);
            await SaveChanges();
            return true;
        }
        public async Task<Configs?> GetConfigById(byte ConfigId)
        {
            return await GetById(ConfigId);
        }
    }
}
