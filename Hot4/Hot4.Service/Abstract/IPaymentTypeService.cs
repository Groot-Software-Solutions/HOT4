using Hot4.DataModel.Models;
using Hot4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Service.Abstract
{
    public interface IPaymentTypeService
    {
        Task<PaymentTypeModel> GetPaymentTypeById(byte PaymentTypeId);
        Task<List<PaymentTypeModel>> ListPaymentType();
        Task<bool> AddPaymentType(PaymentTypeModel paymentTypeModel);
        Task<bool> UpdatePaymentType(PaymentTypeModel paymentTypeModel);
        Task<bool> DeletePaymentType(PaymentTypeModel paymentTypeModel);
    }
}
