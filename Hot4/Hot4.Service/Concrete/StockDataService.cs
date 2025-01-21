using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class StockDataService : IStockDataService
    {
        private readonly IStockDataRepository _stockDataRepository;
        private readonly IMapper Mapper;
        public StockDataService(IStockDataRepository stockDataRepository, IMapper mapper)
        {
            _stockDataRepository = stockDataRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddStockData(StockDataModel stockData)
        {
            var model = Mapper.Map<StockDataModel, StockData>(stockData);
            return await _stockDataRepository.AddStockData(model);
        }

        public async Task<bool> DeleteStockData(byte stockDataId)
        {
            var record = await GetEntityById(stockDataId);
            if (record != null)
            {
                return await _stockDataRepository.DeleteStockData(record);
            }
            return false;
        }

        public async Task<StockDataModel?> GetStockDataById(byte stockDataId)
        {
            var record = await GetEntityById(stockDataId);
            return Mapper.Map<StockDataModel?>(record);
        }

        public async Task<List<StockDataModel>> ListStockData()
        {
            var records = await _stockDataRepository.ListStockData();
            return Mapper.Map<List<StockDataModel>>(records);
        }

        public async Task<bool> UpdateStockData(StockDataModel stockData)
        {
            var record = await GetEntityById(stockData.StockDataId);
            if (record != null)
            {
                var model = Mapper.Map(stockData, record);
                return await _stockDataRepository.UpdateStockData(record);
            }
            return false;
        }
        private async Task<StockData?> GetEntityById(byte stockDataId)
        {
            return await _stockDataRepository.GetStockDataById(stockDataId);
        }
    }
}
