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
        private readonly IMapper Mapper;
        public ChannelService(IChannelRepository channelRepository , IMapper mapper)
        {
            _channelRepository = channelRepository;
            Mapper = mapper;
        }
        public async Task<List<ChannelModel>> ListChannel()
        {
            var records = await _channelRepository.ListChannel();
            return Mapper.Map<List<ChannelModel>>(records); 
        }
        public async Task<bool> AddChannel(ChannelModel channelModel)
        {
            if (channelModel != null)
            {
                var model = Mapper.Map<Channels>(channelModel);
               return await _channelRepository.AddChannel(model);
            }
            return false;
        }
        public async Task<bool> UpdateChannel(ChannelModel channelModel)
        {
            var record = await GetEntityById(channelModel.ChannelId);
            if (record != null)
            {
             Mapper.Map(channelModel, record);
             return  await _channelRepository.UpdateChannel(record);
            }
            return false;
        }
        public async Task<bool> DeleteChannel(byte ChannelId)
        {
            var record = await GetEntityById(ChannelId);
            if (record != null)
            {
              return await _channelRepository.DeleteChannel(record);
            }
            return false;
        }
        public async Task<ChannelModel> GetByChannelId(byte channelId)
        {
            var record = await GetEntityById(channelId);
            return Mapper.Map<ChannelModel>(record);
        }
        private async Task<Channels> GetEntityById(byte ChannelId) 
        {
            return await _channelRepository.GetByChannelId(ChannelId);
        }
    }
}
