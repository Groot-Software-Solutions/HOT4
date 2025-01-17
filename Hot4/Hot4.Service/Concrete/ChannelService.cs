using AutoMapper;
using Hot4.DataModel.Models;
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
        public async Task<List<ChannelModel>> ListChannel()
        {
            var records = await _channelRepository.ListChannel();
            var model = _mapper.Map<List<ChannelModel>>(records);
            return model; 
        }
        public async Task<bool> AddChannel(ChannelModel channelModel)
        {
            if (channelModel != null)
            {
                var model = _mapper.Map<Channels>(channelModel);
               return await _channelRepository.AddChannel(model);
            }
            return false;
        }
        public async Task<bool> UpdateChannel(ChannelModel channelModel)
        {
            var record = await _channelRepository.GetByChannelId(channelModel.ChannelId);
            if (record != null)
            {
                _mapper.Map(channelModel, record);
             return  await _channelRepository.UpdateChannel(record);
            }
            return false;
        }
        public async Task<bool> DeleteChannel(ChannelModel channelModel)
        {
            var record = await _channelRepository.GetByChannelId(channelModel.ChannelId);
            if (record != null)
            {
              return await _channelRepository.DeleteChannel(record);
            }
            return false;
        }
        public async Task<ChannelModel> GetByChannelId(byte channelId)
        {
            var record = await _channelRepository.GetByChannelId(channelId);

            if (record != null)
            {
                return _mapper.Map<ChannelModel>(record);
            }
            return null;
        }
    }
}
