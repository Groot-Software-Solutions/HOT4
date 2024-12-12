using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IRegistrationRepository
    {
        Task Register(Sms sms);
        Task<Access?> RegisterSelfTopUpUser(Sms sms);
    }
}
