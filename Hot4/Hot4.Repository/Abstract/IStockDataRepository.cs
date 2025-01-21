using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IStockDataRepository
    {
        Task<bool> AddStockData(StockData stockData);
        Task<bool> UpdateStockData(StockData stockData);
        Task<StockData?> GetStockDataById(byte stockDataId);
        Task<bool> DeleteStockData(StockData stockData);
        Task<List<StockData>> ListStockData();

    }
}
