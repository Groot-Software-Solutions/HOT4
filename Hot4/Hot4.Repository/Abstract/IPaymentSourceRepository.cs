using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IPaymentSourceRepository
    {
        Task<List<PaymentSourceModel>> ListPaymentSource();
    }
}
