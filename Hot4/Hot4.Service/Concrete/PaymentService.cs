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
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository , IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }
        public async Task<PaymentModel?> GetPaymentById(long paymentId)
        {
            var record = await _paymentRepository.GetPaymentById(paymentId);
            return _mapper.Map<PaymentModel>(record);                      
        }
        public async Task<bool> SaveUpdatePayment(PaymentModel paymentModel)
        {
            if (paymentModel != null)
            {
               var model =  _mapper.Map<Payment>(paymentModel);
               return await _paymentRepository.SaveUpdatePayment(model);
            }
            return false;
        }
    }
}
