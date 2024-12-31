using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IRegistrationOldRepository
    {
        Task Register(Sms sms);
        Task<Access?> RegisterSelfTopUpUser(Sms sms);
    }
}
