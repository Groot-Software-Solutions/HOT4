using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IBankvPaymentService
    {
        Task<bool> AddBankvPayment(BankvPaymentModel bankvPayment);
        Task<bool> UpdateBankvPayment(BankvPaymentModel bankvPayment);
        Task<bool> DeleteBankvPayment(string vPaymentId);
        Task<BankvPaymentModel?> GetBankvPaymentByvPaymentId(string vPaymentId);
    }
}
