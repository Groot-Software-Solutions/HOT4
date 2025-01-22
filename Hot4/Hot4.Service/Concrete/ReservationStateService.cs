using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Service.Concrete
{
    public class ReservationStateService : IReservationStateService
    {
        private readonly IReservationStateRepository _reservationStateRepository;
        private readonly IMapper Mapper;
        public ReservationStateService(IReservationStateRepository reservationStateRepository , IMapper mapper)
        {
            _reservationStateRepository = reservationStateRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddReservationState(ReservationStateModel reservationStateModel)
        {
            if (reservationStateModel != null)
            {
                var model = Mapper.Map<ReservationStates>(reservationStateModel);
                return await _reservationStateRepository.AddReservationState(model);
            }
            return false;
        }
        public async Task<bool> DeleteReservationState(byte reservationStateId)
        {
            var record = await GetEntityById(reservationStateId);
            if (record != null)
            {
               return await _reservationStateRepository.DeleteReservationState(record);
            }
            return false;
        }
        public async Task<ReservationStateModel> GetReservationStateById(byte reservationStateId)
        {
            var record = await GetEntityById(reservationStateId);
            return  Mapper.Map<ReservationStateModel>(record);
        }
        public async Task<List<ReservationStateModel>> ListReservationState()
        {
            var records = await _reservationStateRepository.ListReservationState();
            return Mapper.Map< List<ReservationStateModel>>(records);
        }
        public async Task<bool> UpdateReservationState(ReservationStateModel reservationStateModel)
        {
            var record = await GetEntityById(reservationStateModel.ReservationStateId);
            if (record != null)
            {
                Mapper.Map(reservationStateModel , record);
                return await _reservationStateRepository.UpdateReservationState(record);  
            }
            return false;
        }
        private async Task<ReservationStates> GetEntityById (byte reservationStateId)
        {
            return await _reservationStateRepository.GetReservationStateById(reservationStateId);        } 
    }
}
