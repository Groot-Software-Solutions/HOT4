using Hot4.DataModel.Models;
using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IChannelRepository
    {
        Task<List<ChannelModel>> GetChannel();
        Task AddChannel(Channels channel);
        Task UpdateChannel(Channels channel);
        Task DeleteChannel(Channels channel);
    }
}
