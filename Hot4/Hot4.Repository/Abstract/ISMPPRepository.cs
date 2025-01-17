using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface ISMPPRepository
    {
        Task<List<Smpp>> ListSMPP();
        Task<Smpp?> GetSMPPById(byte smppId);
        Task<bool> AddSMPP(Smpp smpp);
        Task<bool> UpdateSMPP(Smpp smpp);
        Task<bool> DeleteSMPP(Smpp smpp);
    }
}
