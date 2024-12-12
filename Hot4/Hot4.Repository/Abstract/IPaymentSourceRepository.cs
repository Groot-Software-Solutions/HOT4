using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IPaymentSourceRepository
    {
        Task<List<TblPaymentSource>> ListPaymentSource();
    }
}
