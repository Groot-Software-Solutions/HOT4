using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class BankvPaymentService : IBankvPaymentService
    {
        private readonly IBankvPaymentRepository _bankvPaymentRepository;
        private readonly IMapper Mapper;

        public BankvPaymentService(IBankvPaymentRepository bankvPaymentRepository, IMapper mapper)
        {
            _bankvPaymentRepository = bankvPaymentRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddBankvPayment(BankvPaymentModel bankvPayment)
        {
            if (bankvPayment != null)
            {
                var model = Mapper.Map<BankvPayment>(bankvPayment);
                return await _bankvPaymentRepository.AddBankvPayment(model);
            }
            return false;            
        }
        public async Task<bool> DeleteBankvPayment(string vPaymentId)
        {
            var record = await GetEntityById(vPaymentId);
            if (record != null)
            {
                return await _bankvPaymentRepository.DeleteBankvPayment(record);
            }
            return false;
        }
        public async Task<BankvPaymentModel?> GetBankvPaymentByvPaymentId(string vPaymentId)
        {
            var record = await GetEntityById(vPaymentId);
            return Mapper.Map<BankvPaymentModel?>(record);
        }
        public async Task<bool> UpdateBankvPayment(BankvPaymentModel bankvPayment)
        {
            var record = await GetEntityById(bankvPayment.VPaymentId.ToString());
            if (record != null)
            {
                Mapper.Map(bankvPayment, record);
                return await _bankvPaymentRepository.UpdateBankvPayment(record);
            }
            return false;
        }
        private async Task<BankvPayment?> GetEntityById (string vPaymentId)
        {
            return await _bankvPaymentRepository.GetBankvPaymentByvPaymentId(vPaymentId);
        }
    }
}
