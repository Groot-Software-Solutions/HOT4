﻿using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IProductMetaDataTypeRepository
    {
        Task<List<ProductMetaDataTypeModel>> ListProductMetaDataType();
    }
}
