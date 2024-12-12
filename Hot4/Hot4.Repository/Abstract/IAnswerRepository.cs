using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IAnswerRepository
    {
        Task RespondToUnknown(Sms sms);

        Task RespondToAnswer(Sms sms);
    }
}
