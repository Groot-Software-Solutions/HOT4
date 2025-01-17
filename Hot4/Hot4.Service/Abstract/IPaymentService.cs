using Hot4.DataModel.Models;
using Hot4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Service.Abstract
{
    public interface IPaymentService
    {
        Task<PaymentModel?> GetPaymentById(long paymentId);
        Task<bool> SaveUpdatePayment(PaymentModel paymentModel);
    }
}
