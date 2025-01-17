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
        public Task<bool> AddPaymentType(PaymentTypeModel paymentTypeModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePaymentType(PaymentTypeModel paymentTypeModel)
        {
            throw new NotImplementedException();
        }

        public Task<PaymentTypeModel> GetPaymentTypeById(byte PaymentTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<List<PaymentTypeModel>> ListPaymentType()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePaymentType(PaymentTypeModel paymentTypeModel)
        {
            throw new NotImplementedException();
        }
    }
}
