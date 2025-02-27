﻿using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class TransferService : ITransferService
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IMapper Mapper;
        public TransferService(ITransferRepository transferRepository, IMapper mapper)
        {
            _transferRepository = transferRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddTransfer(TransferRecord transfer)
        {
            if (transfer == null)
            {
                var model = Mapper.Map<Transfer>(transfer);
                return await _transferRepository.AddTransfer(model);
            }
            return false;
            
        }
        public async Task<bool> DeleteTransfer(long transferId)
        {
            var record = await GetEntityById(transferId);
            if (record != null)
            {
                return await _transferRepository.DeleteTransfer(record);
            }
            return false;
        }
        public async Task<StockTradeModel> GetStockTrade(StockTradeSearchModel stockTradeSearch)
        {
            var records = await _transferRepository.GetStockTrade(stockTradeSearch);
            return Mapper.Map<StockTradeModel>(records);
        }
        public async Task<decimal> GetStockTradeBalByAccountId(long accountId)
        {
            return await _transferRepository.GetStockTradeBalByAccountId(accountId);
        }
        public async Task<TransferModel> GetTransferById(long transferId)
        {
            var record = await GetEntityById(transferId);
            return Mapper.Map<TransferModel>(record);
        }
        public async Task<List<TransferModel>> GetTransferByPaymentId(long paymentId)
        {
            var records = await _transferRepository.GetTransferByPaymentId(paymentId);
            return Mapper.Map<List<TransferModel>>(records);
        }
        public async Task<List<TransferModel>> ListTransfer(int pageNo, int pageSize)
        {
            var records = await _transferRepository.ListTransfer(pageNo, pageSize);
            return Mapper.Map<List<TransferModel>>(records);
        }
        public async Task<bool> UpdateTransfer(TransferRecord transfer)
        {
            var record = await GetEntityById(transfer.TransferId);
            if (record != null)
            {
                var model = Mapper.Map(transfer, record);
                return await _transferRepository.UpdateTransfer(record);
            }
            return false;
        }
        private async Task<Transfer?> GetEntityById(long TransferId)
        {
            return await _transferRepository.GetTransferById(TransferId);
        }
    }
}
