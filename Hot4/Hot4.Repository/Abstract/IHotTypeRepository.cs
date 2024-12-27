﻿using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IHotTypeRepository
    {

        Task<List<HotTypeModel>> ListHotType();
        Task<byte?> GetHotTypeIdentity(string typeCode, byte splitCount);
    }
}
