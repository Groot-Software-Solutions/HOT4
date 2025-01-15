using AutoMapper;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class ChannelService : IChannelService
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IMapper _mapper;


        public ChannelService(IChannelRepository channelRepository , IMapper mapper)
        {
            _channelRepository = channelRepository;
            _mapper = mapper;
        }

        public Task<bool> AddChannel(ChannelModel channelModel)
        {
            throw new NotImplementedException();
        }

        public Task DeleteChannel(ChannelModel channelModel)
        {
            throw new NotImplementedException();
        }

        public Task<List<ChannelModel>> ListChannel()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateChannel(ChannelModel channelModel)
        {
            throw new NotImplementedException();
        }
    }
}
