using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class SelfTopUpStateService : ISelfTopUpStateService
    {
        private readonly ISelfTopUpStateRepository _selfTopUpStateRepository;
        private readonly IMapper Mapper;
        public SelfTopUpStateService(ISelfTopUpStateRepository selfTopUpStateRepository, IMapper mapper)
        {
            _selfTopUpStateRepository = selfTopUpStateRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddSelfTopUpState(SelfTopUpStateModel selfTopUpState)
        {
            var model = Mapper.Map<SelfTopUpState>(selfTopUpState);
            return await _selfTopUpStateRepository.AddSelfTopUpState(model);
        }

        public async Task<bool> DeleteSelfTopUpState(byte selfTopUpStateId)
        {
            var record = await GetEntityById(selfTopUpStateId);
            if (record != null)
            {
                return await _selfTopUpStateRepository.DeleteSelfTopUpState(record);
            }
            return false;
        }

        public async Task<SelfTopUpStateModel?> GetSelfTopUpStateById(byte selfTopUpStateId)
        {
            var record = await GetEntityById(selfTopUpStateId);
            return Mapper.Map<SelfTopUpStateModel?>(record);
        }

        public async Task<List<SelfTopUpStateModel>> ListSelfTopUpState()
        {
            var records = await _selfTopUpStateRepository.ListSelfTopUpState();
            return Mapper.Map<List<SelfTopUpStateModel>>(records);
        }

        public async Task<bool> UpdateSelfTopUpState(SelfTopUpStateModel selfTopUpState)
        {
            var record = await GetEntityById(selfTopUpState.SelfTopUpStateId);
            if (record != null)
            {
                Mapper.Map(selfTopUpState, record);
                return await _selfTopUpStateRepository.UpdateSelfTopUpState(record);
            }
            return false;
        }
        private async Task<SelfTopUpState?> GetEntityById(byte selfTopUpStateId)
        {
            return await _selfTopUpStateRepository.GetSelfTopUpStateById(selfTopUpStateId);
        }
    }
}
