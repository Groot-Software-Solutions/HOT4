using Hot4.DataModel.Models;
using Hot4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Service.Abstract
{
    public interface IPaymentSourceService
    {
        Task<PaymentSourceModel> GetPaymentSourceById(byte paymentSourceId);
        Task<List<PaymentSourceModel>> ListPaymentSource();
        Task<bool> AddPaymentSource(PaymentSourceModel paymentSourceModel);
        Task<bool> UpdatePaymentSource(PaymentSourceModel paymentSourceModel);
        Task<bool> DeletePaymentSource(byte paymentSourceId);
    }
}
