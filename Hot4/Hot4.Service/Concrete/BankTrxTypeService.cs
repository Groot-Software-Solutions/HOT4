﻿using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class BankTrxTypeService : IBankTrxTypeService
    {
        private readonly IBankTrxTypeRepository _bankTrxTypeRepository;
        private readonly IMapper Mapper;
        public BankTrxTypeService(IBankTrxTypeRepository bankTrxTypeRepository, IMapper mapper)
        {
            _bankTrxTypeRepository = bankTrxTypeRepository;
            Mapper = mapper;
        }
        public async Task<List<BankTransactionTypeModel>> ListBankTrxType()
        {
            var records = await _bankTrxTypeRepository.ListBankTrxType();
            return Mapper.Map<List<BankTransactionTypeModel>>(records);
        }
        public async Task<BankTransactionTypeModel?> GetBankTrxTypeById(byte bankTrxTypeId)
        {
            var record = await GetEntityById(bankTrxTypeId);
            return Mapper.Map<BankTransactionTypeModel?>(record);
        }
        public async Task<bool> AddBankTrxType(BankTransactionTypeModel bankType)
        {
            if (bankType != null)
            {
                var model = Mapper.Map<BankTrxTypes>(bankType);
                return await _bankTrxTypeRepository.AddBankTrxType(model);
            }
            return false;          
        }
        public async Task<bool> DeleteBankTrxType(byte bankTrxTypeId)
        {
            var record = await GetEntityById(bankTrxTypeId);
            if (record != null)
            {
                return await _bankTrxTypeRepository.DeleteBankTrxType(record);
            }
            return false;
        }
        public async Task<bool> UpdateBankTrxType(BankTransactionTypeModel bankType)
        {
            var record = await GetEntityById(bankType.BankTrxTypeId);
            if (record != null)
            {
                Mapper.Map(bankType, record);
                return await _bankTrxTypeRepository.UpdateBankTrxType(record);
            }
            return false;
        }
        private async Task<BankTrxTypes?> GetEntityById (byte bankTrxTypeId)
        {
            return await _bankTrxTypeRepository.GetBankTrxTypeById(bankTrxTypeId);
        }
    }
}
