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
        private readonly IMapper Mapper;

        public PaymentService(IPaymentRepository paymentRepository , IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            Mapper = mapper;
        }
        public async Task<PaymentModel?> GetPaymentById(long paymentId)
        {
            var record = await _paymentRepository.GetPaymentById(paymentId);
            return Mapper.Map<PaymentModel>(record);                      
        }
        public async Task<bool> SaveUpdatePayment(PaymentModel paymentModel)
        {
            if (paymentModel != null)
            {
               var model =  Mapper.Map<Payment>(paymentModel);
               return await _paymentRepository.SaveUpdatePayment(model);
            }
            return false;
        }

    }
}
