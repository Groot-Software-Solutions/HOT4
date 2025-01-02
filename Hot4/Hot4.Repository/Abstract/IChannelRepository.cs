using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IChannelRepository
    {
        Task AddChannel(Channels channel);
        Task UpdateChannel(Channels channel);
        Task DeleteChannel(Channels channel);
        Task<List<ChannelModel>> ListChannel();
    }
}
