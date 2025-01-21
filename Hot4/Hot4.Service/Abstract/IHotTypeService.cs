using Hot4.DataModel.Models;
using Hot4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Service.Abstract
{
    public interface IHotTypeService
    {
        Task<HotTypeModel> GetHotTypeById(byte HotTypeId);
        Task<List<HotTypeModel>> ListHotType();
        Task<byte?> GetHotTypeIdentity(string typeCode, byte splitCount);
        Task<bool> AddHotType(HotTypeModel hotTypeModel);
        Task<bool> UpdateHotType(HotTypeModel hotTypeModel);
        Task<bool> DeleteHotType(byte HotTypeId);
    }
}
