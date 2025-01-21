using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IStockDataService
    {
        Task<bool> AddStockData(StockDataModel stockData);
        Task<bool> UpdateStockData(StockDataModel stockData);
        Task<StockDataModel?> GetStockDataById(byte stockDataId);
        Task<bool> DeleteStockData(byte stockDataId);
        Task<List<StockDataModel>> ListStockData();
    }
}
