﻿using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IChannelService
    {
        Task<List<ChannelModel>> ListChannel();
        Task<bool> AddChannel(ChannelModel channelModel);
        Task<bool> UpdateChannel(ChannelModel channelModel);
        Task<bool> DeleteChannel(ChannelModel channelModel);
        
    }
}
