using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel.ApiModels;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class ConfigRepository : RepositoryBase<Configs>, IConfigRepository
    {
        public ConfigRepository(HotDbContext context) : base(context) { }
        public async Task<List<ConfigModel>> GetConfig()
        {
            return await GetAll().
                Select(d => new ConfigModel
                {
                    ConfigId = d.ConfigId,
                    ProfileIdNewSmsdealer = d.ProfileIdNewSmsdealer,
                    ProfileIdNewWebDealer = d.ProfileIdNewWebDealer,
                    MaxRecharge = d.MaxRecharge,
                    MinRecharge = d.MinRecharge,
                    MinTransfer = d.MinTransfer,
                    PrepaidEnabled = d.PrepaidEnabled,
                }).ToListAsync();
        }
    }
}
