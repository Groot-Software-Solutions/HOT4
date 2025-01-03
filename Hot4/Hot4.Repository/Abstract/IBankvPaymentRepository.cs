using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IBankvPaymentRepository
    {
        Task AddBankvPayment(BankvPayment bankvPayment);
        Task UpdateBankvPayment(BankvPayment bankvPayment);
        Task DeleteBankvPayment(BankvPayment bankvPayment);
        Task<BankvPaymentModel?> GetBankvPaymentByvPaymentId(string vPaymentId);
    }
}
