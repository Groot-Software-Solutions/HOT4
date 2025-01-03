using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IStockDataRepository
    {
        Task AddStockData(StockData stockData);
        Task UpdateStockData(StockData stockData);
        Task DeleteStockData(StockData stockData);
        Task<List<StockDataModel>> ListStockData();
    }
}
