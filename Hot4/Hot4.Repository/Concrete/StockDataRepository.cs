using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class StockDataRepository : RepositoryBase<StockData>, IStockDataRepository
    {
        public StockDataRepository(HotDbContext context) : base(context) { }
        public async Task<bool> AddStockData(StockData stockData)
        {
            await Create(stockData);
            await SaveChanges();
            return true;
        }

        //public async Task<bool> DeleteStockData(StockData stockData)
        //{
        //    Delete(stockData);
        //    await SaveChanges();
        //    return true;
        //}

        public async Task<List<StockData>> ListStockData()
        {
            return await GetAll().ToListAsync();
            //{
            //    Available = d.Available,
            //    BrandName = d.BrandName,
            //    LastSold = d.LastSold,
            //    MonthSold = d.MonthSold,
            //    PinValue = d.PinValue,
            //    Sold = d.Sold,
            //    WeekSold = d.WeekSold,
            //}).ToListAsync();
        }

        //public async Task<bool> UpdateStockData(StockData stockData)
        //{
        //    Update(stockData);
        //    await SaveChanges();
        //    return true;
        //}
    }
}
