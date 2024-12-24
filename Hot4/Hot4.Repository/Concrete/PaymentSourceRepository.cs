using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel.ApiModels;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class PaymentSourceRepository : RepositoryBase<PaymentSources>, IPaymentSourceRepository
    {
        public PaymentSourceRepository(HotDbContext context) : base(context) { }
        public async Task<List<PaymentSourceModel>> ListPaymentSource()
        {
            return await GetAll()
                .Select(d => new PaymentSourceModel
                {
                    PaymentSourceId = d.PaymentSourceId,
                    PaymentSource = d.PaymentSource,
                    PaymentSourceText = d.PaymentSource,
                    WalletTypeId = d.WalletTypeId,
                })
                .ToListAsync();
        }
    }
}
