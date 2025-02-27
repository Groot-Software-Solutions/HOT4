﻿using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class StateService : IStateService
    {
        private readonly IStateRepository _stateRepository;
        private readonly IMapper Mapper;
        public StateService(IStateRepository stateRepository, IMapper mapper)
        {
            _stateRepository = stateRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddState(StateModel state)
        {
            if (state != null) 
            {
                var model = Mapper.Map<States>(state);
                return await _stateRepository.AddState(model);
            }
            return false;
        }
        public async Task<bool> DeleteState(byte stateId)
        {
            var record = await GetEntityById(stateId);
            if (record != null)
            {
                return await _stateRepository.DeleteState(record);
            }
            return false;
        }
        public async Task<StateModel?> GetStateById(byte stateId)
        {
            var record = await GetEntityById(stateId);
            return Mapper.Map<StateModel>(record);
        }
        public async Task<List<StateModel>> ListState()
        {
            var records = await _stateRepository.ListState();
            return Mapper.Map<List<StateModel>>(records);
        }
        public async Task<bool> UpdateState(StateModel state)
        {
            var record = await GetEntityById(state.StateId);
            if (record != null)
            {
                var model = Mapper.Map(state, record);
                return await _stateRepository.UpdateState(record);
            }
            return false;
        }
        private async Task<States?> GetEntityById (byte StateId)
        {
            return await _stateRepository.GetStateById(StateId);
        }
    }
}
