

using Hot4.Core.DataViewModels;

namespace Hot4.Repository.Abstract
{
    public interface ISMPPRepository
    {
        Task<List<SMPPModel>> ListSMPP();
    }
}
