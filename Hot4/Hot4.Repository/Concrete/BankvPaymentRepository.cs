using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;

namespace Hot4.Repository.Concrete
{
    public class BankvPaymentRepository : RepositoryBase<BankvPayment>, IBankvPaymentRepository
    {
        public BankvPaymentRepository(HotDbContext context) : base(context) { }
        public async Task AddBankvPayment(BankvPayment bankvPayment)
        {
            await Create(bankvPayment);
            await SaveChanges();
        }

        public async Task DeleteBankvPayment(BankvPayment bankvPayment)
        {
            Delete(bankvPayment);
            await SaveChanges();
        }

        public async Task<BankvPaymentModel?> GetBankvPaymentByvPaymentId(string vPaymentId)
        {
            var result = await GetById(vPaymentId);
            if (result != null)
            {
                return new BankvPaymentModel
                {
                    BankTrxId = result.BankTrxId,
                    CheckUrl = result.CheckUrl,
                    ErrorMsg = result.ErrorMsg,
                    ProcessUrl = result.ProcessUrl,
                    VPaymentId = result.VPaymentId,
                    VPaymentRef = result.VPaymentRef
                };
            }
            return null;
        }

        public async Task UpdateBankvPayment(BankvPayment bankvPayment)
        {
            Update(bankvPayment);
            await SaveChanges();
        }
    }
}
