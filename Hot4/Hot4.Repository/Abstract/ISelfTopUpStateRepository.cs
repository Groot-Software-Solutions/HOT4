using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface ISelfTopUpStateRepository
    {
        Task<long> AddSelfTopUpState(SelfTopUpState selfTopUpState);
        Task UpdateSelfTopUpState(SelfTopUpState selfTopUpState);
        Task DeleteSelfTopUpState(SelfTopUpState selfTopUpState);
        Task<List<SelfTopUpStateModel>> ListSelfTopUpState();
    }
}
