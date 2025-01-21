using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Service.Concrete
{
    public class PaymentTypeService : IPaymentTypeService
    {
        private readonly IPaymentTypeRepository _paymentTypeRepository;
        private readonly IMapper Mapper;
        public PaymentTypeService(IPaymentTypeRepository paymentTypeRepository , IMapper mapper)
        {
            _paymentTypeRepository = paymentTypeRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddPaymentType(PaymentTypeModel paymentTypeModel)
        {
            if (paymentTypeModel != null)
            {
                var model = Mapper.Map<PaymentTypes>(paymentTypeModel);
              return await _paymentTypeRepository.AddPaymentType(model);
            }
            return false;
        }
        public async Task<bool> DeletePaymentType(byte PaymentTypeId)
        {
            var record = await GetEntityById(PaymentTypeId);
            if (record != null)
            {
              return await _paymentTypeRepository.DeletePaymentType(record);
            }
            return false;
        }
        public async Task<PaymentTypeModel> GetPaymentTypeById(byte PaymentTypeId)
        {
            var record = await GetEntityById(PaymentTypeId);
            return Mapper.Map<PaymentTypeModel>(record);
        }
        public async Task<List<PaymentTypeModel>> ListPaymentType()
        {
            var records = await _paymentTypeRepository.ListPaymentType();
            return Mapper.Map<List<PaymentTypeModel>>(records);
        }
        public async Task<bool> UpdatePaymentType(PaymentTypeModel paymentTypeModel)
        {
            var record = await GetEntityById(paymentTypeModel.PaymentTypeId); 
            if (record != null)
            {
                Mapper.Map(paymentTypeModel , record);
                return await _paymentTypeRepository.UpdatePaymentType(record);
            }
            return false;
        }
        private async Task<PaymentTypes> GetEntityById (byte PaymentTypeId)
        {
            return await _paymentTypeRepository.GetPaymentTypeById(PaymentTypeId);
        }
    }
}
