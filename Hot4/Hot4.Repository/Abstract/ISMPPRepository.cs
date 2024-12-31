using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface ISMPPRepository
    {
        Task<List<SMPPModel>> ListSMPP();
        Task AddSMPP(Smpp smpp);
        Task UpdateSMPP(Smpp smpp);
    }
}
