using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IAnswerRepository
    {
        Task RespondToUnknown(TblSms sms);

        Task RespondToAnswer(TblSms sms);
    }
}
