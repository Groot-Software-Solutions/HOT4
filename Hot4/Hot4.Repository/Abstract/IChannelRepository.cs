using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IChannelRepository
    {
        Task<List<TblChannel>> GetChannels();
    }
}
