using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IChannelRepository
    {
        Task <Channels> GetByChannelId (byte channelId);
        Task <bool>AddChannel(Channels channel);
        Task <bool>UpdateChannel(Channels channel);
        Task<bool> DeleteChannel(Channels channel);
        Task<List<Channels>> ListChannel();
    }
}
