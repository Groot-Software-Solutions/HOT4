using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IChannelRepository
    {
        Task<List<Channels>> GetChannels();
    }
}
