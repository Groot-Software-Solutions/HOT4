using Hot4.DataModel.Models;
using Hot4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Service.Abstract
{
    public interface IConfigService
    {
        Task<ConfigModel> GetConfigById(byte ConfigId);
        Task<List<ConfigModel>> ListConfig();
        Task<bool> AddConfig(ConfigModel configModel);
        Task<bool> UpdateConfig(ConfigModel configModel);
        Task<bool> DeleteConfig(ConfigModel configModel);
    }
}
