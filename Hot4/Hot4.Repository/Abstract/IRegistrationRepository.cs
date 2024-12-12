using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IRegistrationRepository
    {
        Task Register(TblSms sms);
        Task<Access?> RegisterSelfTopUpUser(TblSms sms);
    }
}
