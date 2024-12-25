using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IBankRepository
    {
        Task<List<BankModel>> ListBanks();
    }
}
