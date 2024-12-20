using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IBankRepository
    {
        Task<List<Banks>> ListBanks();
    }
}
