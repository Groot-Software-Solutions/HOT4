using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface ISMPPService
    {
        Task<List<SMPPModel>> ListSMPP();
        Task<SMPPModel?> GetSMPPById(byte smppId);
        Task<bool> AddSMPP(SMPPModel smpp);
        Task<bool> UpdateSMPP(SMPPModel smpp);
        Task<bool> DeleteSMPP(byte smppId);
    }
}
