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
    public class ReservationLogService : IReservationLogService
    {
        private readonly IReservationLogRepository _reservationLogRepository;
        private readonly IMapper Mapper;
        public ReservationLogService(IReservationLogRepository reservationLogRepository , IMapper mapper)
        {
            _reservationLogRepository = reservationLogRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddReservationLog(ReservationLogModel reservationLogModel)
        {
            if (reservationLogModel != null)
            {
                var model = Mapper.Map<ReservationLog>(reservationLogModel);
                return await _reservationLogRepository.AddReservationLog(model);
            }
            return false;
        }

        public async Task<bool> DeleteReservationLog(ReservationLogModel reservationLogModel)
        {
            var record = await getEntityById(reservationLogModel.ReservationId);
            if (record != null)
            {
                return await _reservationLogRepository.DeleteReservationLog(record);
            }
            return false;
        }

        public async Task<ReservationLogModel> GetReservationLogById(long ReservationLogId)
        {
            var record = await getEntityById(ReservationLogId);
            return Mapper.Map<ReservationLogModel>(record);
        }

        public async Task<List<ReservationLogModel>> ListReservationLog(int pageNo, int pageSize)
        {
            var records = await _reservationLogRepository.ListReservationLog(pageNo, pageSize);
            return  Mapper.Map<List<ReservationLogModel>>(records);
        }

        public async Task<bool> UpdateReservationLog(ReservationLogModel reservationLogModel)
        {
            var record = await getEntityById(reservationLogModel.ReservationId);
            if (record != null)
            {
                Mapper.Map(reservationLogModel , record);
                return await _reservationLogRepository.UpdateReservationLog(record);
            }
            return true;
        }

        private async Task<ReservationLog> getEntityById (long ReservationLogId)
        {
            return await _reservationLogRepository.GetReservationLogById(ReservationLogId);
        }
    }
}
