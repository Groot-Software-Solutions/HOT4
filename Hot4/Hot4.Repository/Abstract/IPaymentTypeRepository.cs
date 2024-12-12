using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IPaymentTypeRepository
    {
        Task<List<TblPaymentType>> ListPaymentType();
    }
}
