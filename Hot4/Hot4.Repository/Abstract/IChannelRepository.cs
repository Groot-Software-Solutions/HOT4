using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IChannelRepository
    {
        Task<List<ChannelModel>> GetChannels();
    }
}
