using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class StockDataRepository : RepositoryBase<StockData>, IStockDataRepository
    {
        public StockDataRepository(HotDbContext context) : base(context) { }
        public async Task AddStockData(StockData stockData)
        {
            await Create(stockData);
            await SaveChanges();
        }

        public async Task DeleteStockData(StockData stockData)
        {
            await Delete(stockData);
            await SaveChanges();
        }

        public async Task<List<StockDataModel>> ListStockData()
        {
            return await GetAll().Select(d => new StockDataModel
            {
                Available = d.Available,
                BrandName = d.BrandName,
                LastSold = d.LastSold,
                MonthSold = d.MonthSold,
                PinValue = d.PinValue,
                Sold = d.Sold,
                WeekSold = d.WeekSold,
            }).ToListAsync();
        }

        public async Task UpdateStockData(StockData stockData)
        {
            await Update(stockData);
            await SaveChanges();
        }
    }
}
