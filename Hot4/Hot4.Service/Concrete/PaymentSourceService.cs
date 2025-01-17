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
    public class PaymentSourceService : IPaymentSourceService
    {
        private readonly IPaymentSourceRepository _paymentSourceRepository;
        private readonly IMapper _mapper;

        public PaymentSourceService(IPaymentSourceRepository paymentSourceRepository, IMapper mapper)
        {
            _paymentSourceRepository = paymentSourceRepository;
            _mapper = mapper;
        }
        public async Task<bool> AddPaymentSource(PaymentSourceModel paymentSourceModel)
        {
            if (paymentSourceModel != null )
            {
                var model = _mapper.Map<PaymentSources>(paymentSourceModel);
                return await _paymentSourceRepository.AddPaymentSource(model);
            }
            return false;
        }

        public async Task<bool> DeletePaymentSource(PaymentSourceModel paymentSourceModel)
        {
            var record = await GetEntityById(paymentSourceModel.PaymentSourceId);
            if (record != null)
            {
                return await _paymentSourceRepository.DeletePaymentSource(record);
            }
            return false;
        }

        public async Task<PaymentSourceModel> GetPaymentSourceById(byte PaymentSourceId)
        {
            var record = await GetEntityById(PaymentSourceId);
            return _mapper.Map<PaymentSourceModel>(record);
        }

        // need to  chcek ListPaymentSource()
        public Task<List<PaymentSourceModel>> ListPaymentSource()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdatePaymentSource(PaymentSourceModel paymentSourceModel)
        {
            var record = await GetEntityById(paymentSourceModel.PaymentSourceId);
            if (record != null)
            {
                _mapper.Map(paymentSourceModel, record);
                return await _paymentSourceRepository.UpdatePaymentSource(record);
            }
            return false; 
        }

        private async Task<PaymentSources> GetEntityById(byte PaymentSourceId)
        {
            return await _paymentSourceRepository.GetPaymentSourceById(PaymentSourceId);
        }
    }
}
