using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IBankvPaymentRepository
    {
        Task<bool> AddBankvPayment(BankvPayment bankvPayment);
        Task<bool> UpdateBankvPayment(BankvPayment bankvPayment);
        Task<bool> DeleteBankvPayment(BankvPayment bankvPayment);
        Task<BankvPayment?> GetBankvPaymentByvPaymentId(string vPaymentId);
    }
}
