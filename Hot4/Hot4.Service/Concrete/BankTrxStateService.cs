﻿using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class BankTrxStateService : IBankTrxStateService
    {
        private readonly IBankTrxStateRepository _bankTrxStateRepository;
        private readonly IMapper Mapper;
        public BankTrxStateService(IBankTrxStateRepository bankTrxStateRepository, IMapper mapper)
        {
            _bankTrxStateRepository = bankTrxStateRepository;
            Mapper = mapper;
        }
        public async Task<List<BankTransactionStateModel>> ListBankTrxStates()
        {
            var records = await _bankTrxStateRepository.ListBankTrxStates();
            return Mapper.Map<List<BankTransactionStateModel>>(records);
        }
        public async Task<BankTransactionStateModel?> GetBankTrxStateById(byte bankTrxStateId)
        {
            var record = await GetEntityById(bankTrxStateId);
            return Mapper.Map<BankTransactionStateModel>(record);
        }
        public async Task<bool> AddBankTrxState(BankTransactionStateModel bankState)
        {
            if (bankState != null)
            {
                var model = Mapper.Map<BankTrxStates>(bankState);
                return await _bankTrxStateRepository.AddBankTrxState(model);
            }
            return false;
            
        }
        public async Task<bool> UpdateBankTrxState(BankTransactionStateModel bankState)
        {
            var record = await GetEntityById(bankState.BankTrxStateId);
            if (record != null)
            {
                Mapper.Map(bankState, record);
                return await _bankTrxStateRepository.UpdateBankTrxState(record);
            }
            return false;
        }
        public async Task<bool> DeleteBankTrxState(byte bankTrxStateId)
        {
            var record = await GetEntityById(bankTrxStateId);
            if (record != null)
            {
                return await _bankTrxStateRepository.DeleteBankTrxState(record);
            }
            return false;
        }
        private async Task<BankTrxStates?> GetEntityById (byte bankTrxStateId)
        {
            return await _bankTrxStateRepository.GetBankTrxStateById(bankTrxStateId);
        }
    }
}
