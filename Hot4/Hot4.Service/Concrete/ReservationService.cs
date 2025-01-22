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
    public class ReservationService : IReservationService

    { 
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper Mapper;
        public ReservationService(IReservationRepository reservationRepository , IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddReservation(ReservationModel reservationModel)
        {
            if (reservationModel != null)
            {
              var Model =Mapper.Map<Reservation>(reservationModel);
                return await _reservationRepository.AddReservation(Model);
            }
            return false;
        }
        public async Task<bool> DeleteReservation(long reservationId)
        {
            var record = await GetEntityById(reservationId);
            if (record != null) 
            {
               return await _reservationRepository.DeleteReservation(record);
            }
            return false;
        }
        public  async Task<ReservationModel> GetReservationById(long reservationId)
        {
            var record = await GetEntityById(reservationId);
            return Mapper.Map<ReservationModel>(record);
        }
        public async Task<List<ReservationModel>> GetReservationByRechargeId(long rechargeId)
        {
            var records = await _reservationRepository.GetReservationByRechargeId(rechargeId);
            return Mapper.Map<List<ReservationModel>>(records);
        }
        public async Task<bool> UpdateReservation(ReservationModel reservationModel)
        {
            var record = await GetEntityById(reservationModel.ReservationId);
            if (record != null) 
            {
                Mapper.Map(reservationModel, record);
                await _reservationRepository.UpdateReservation(record);
            }
            return true;
        }
        private async Task<Reservation> GetEntityById (long reservationId)
        {
            return await _reservationRepository.GetReservationById(reservationId);
        }
    }
}
