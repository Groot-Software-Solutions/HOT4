using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class NetworkService : INetworkService
    {
        private readonly INetworkRepository _networkRepository;
        private readonly IMapper Mapper;
        public NetworkService(INetworkRepository networkRepository, IMapper mapper)
        {
            _networkRepository = networkRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddNetwork(NetworkModel networks)
        {
            var model = Mapper.Map<Networks>(networks);
            return await _networkRepository.AddNetwork(model);
        }
        public async Task<bool> UpdateNetwork(NetworkModel networks)
        {
            var record = await GetEntityById(networks.NetworkId);
            if (record != null)
            {
                var model = Mapper.Map(networks, record);
                return await _networkRepository.UpdateNetwork(record);
            }
            return false;
        }
        public async Task<NetworkModel?> GetNetworkById(byte networkId)
        {
            var record = await GetEntityById(networkId);
            return Mapper.Map<NetworkModel?>(record);
        }
        public async Task<bool> DeleteNetwork(byte networkId)
        {
            var record = await GetEntityById(networkId);
            if (record != null)
            {
                return await _networkRepository.DeleteNetwork(record);
            }
            return false;
        }

        public async Task<List<NetworkBalanceModel>> GetBrandNetworkBalance()
        {
            return await _networkRepository.GetBrandNetworkBalance();
        }

        public async Task<List<NetworkModel>> GetNetworkIdentityByMobile(string mobile)
        {
            var records = await _networkRepository.GetNetworkIdentityByMobile(mobile);
            return Mapper.Map<List<NetworkModel>>(records);
        }

        private async Task<Networks?> GetEntityById(byte networkId)
        {
            return await _networkRepository.GetNetworkById(networkId);
        }
    }
}
