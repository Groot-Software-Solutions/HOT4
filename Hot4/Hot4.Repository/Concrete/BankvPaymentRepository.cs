using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;

namespace Hot4.Repository.Concrete
{
    public class BankvPaymentRepository : RepositoryBase<BankvPayment>, IBankvPaymentRepository
    {
        public BankvPaymentRepository(HotDbContext context) : base(context) { }
        public async Task<bool> AddBankvPayment(BankvPayment bankvPayment)
        {
            await Create(bankvPayment);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteBankvPayment(BankvPayment bankvPayment)
        {
            Delete(bankvPayment);
            await SaveChanges();
            return true;
        }
        public async Task<BankvPayment?> GetBankvPaymentByvPaymentId(string vPaymentId)
        {
            return await GetById(vPaymentId);
        }
        public async Task<bool> UpdateBankvPayment(BankvPayment bankvPayment)
        {
            Update(bankvPayment);
            await SaveChanges();
            return true;
        }
    }
}
