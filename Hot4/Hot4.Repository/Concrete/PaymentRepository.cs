using Hot4.Core.Enums;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
    {
        public PaymentRepository(HotDbContext context) : base(context) { }
        public async Task<PaymentModel?> GetPaymentById(long paymentId)
        {
            var result = await _context.Payment
                         .Include(d => d.PaymentType)
                         .Include(d => d.PaymentSource)
                         .FirstOrDefaultAsync(d => d.PaymentId == paymentId);
            if (result != null)
            {
                return new PaymentModel
                {
                    PaymentId = result.PaymentId,
                    AccountId = result.AccountId,
                    PaymentSourceId = result.PaymentSourceId,
                    PaymentSource = result.PaymentSource.PaymentSource,
                    Amount = result.Amount,
                    LastUser = result.LastUser,
                    PaymentDate = result.PaymentDate,
                    PaymentType = result.PaymentType.PaymentType,
                    PaymentTypeId = result.PaymentTypeId,
                    Reference = result.Reference
                };
            }
            return null;
        }
        public async Task<long?> SaveUpdatePayment(Payment payment)
        {
            var paymentId = 0;
            if (payment.PaymentTypeId == (int)PaymentMethodType.BankAuto && payment.PaymentSourceId == (int)PaymentMethodSource.EcoCash)
            {
                paymentId = await GetByCondition(d => d.PaymentTypeId == (int)PaymentMethodType.BankAuto
                                  && d.PaymentSourceId == (int)PaymentMethodSource.EcoCash
                                  && d.Reference == payment.Reference && Math.Round(d.Amount, 2) == Math.Round(payment.Amount)
                                  && d.AccountId == payment.AccountId)
                                 .CountAsync();
            }
            if (paymentId == 0)
            {
                payment.PaymentDate = DateTime.Now;
                await Create(payment);
                await SaveChanges();
                return payment.PaymentId;
            }
            else
            {
                var paymentExst = await GetById(paymentId);
                if (paymentExst != null)
                {
                    paymentExst.AccountId = payment.AccountId;
                    paymentExst.PaymentTypeId = payment.PaymentTypeId;
                    paymentExst.PaymentSourceId = payment.PaymentSourceId;
                    paymentExst.Amount = payment.Amount;
                    paymentExst.PaymentDate = DateTime.Now;
                    paymentExst.Reference = payment.Reference;
                    paymentExst.LastUser = payment.LastUser;

                    Update(paymentExst);
                    await SaveChanges();
                    return paymentExst.PaymentId;
                }
                return null;
            }
        }
    }
}
