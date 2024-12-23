﻿namespace Hot4.Repository.Abstract
{
    public interface ICommonRepository
    {
        Task<float> GetPrePaidStockBalance(int brandId);
        Task<decimal> GetBalance(long accountId);
        Task<decimal> GetSaleValue(long accountId);
    }
}
