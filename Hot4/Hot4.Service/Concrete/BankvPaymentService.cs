using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class BankvPaymentService : IBankvPaymentService
    {
        private IBankvPaymentRepository _bankvPaymentRepository;
        private readonly IMapper Mapper;

        public BankvPaymentService(IBankvPaymentRepository bankvPaymentRepository, IMapper mapper)
        {
            _bankvPaymentRepository = bankvPaymentRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddBankvPayment(BankvPaymentModel bankvPayment)
        {
            var model = Mapper.Map<BankvPayment>(bankvPayment);
            return await _bankvPaymentRepository.AddBankvPayment(model);
        }

        public async Task<bool> DeleteBankvPayment(string vPaymentId)
        {
            var record = await _bankvPaymentRepository.GetBankvPaymentByvPaymentId(vPaymentId);
            if (record != null)
            {
                return await _bankvPaymentRepository.DeleteBankvPayment(record);
            }
            return false;
        }

        public async Task<BankvPaymentModel?> GetBankvPaymentByvPaymentId(string vPaymentId)
        {
            var record = await _bankvPaymentRepository.GetBankvPaymentByvPaymentId(vPaymentId);
            return Mapper.Map<BankvPaymentModel?>(record);
        }

        public async Task<bool> UpdateBankvPayment(BankvPaymentModel bankvPayment)
        {
            var record = await _bankvPaymentRepository.GetBankvPaymentByvPaymentId(bankvPayment.VPaymentId.ToString());
            if (record != null)
            {
                Mapper.Map(bankvPayment, record);
                return await _bankvPaymentRepository.UpdateBankvPayment(record);
            }
            return false;
        }
    }
}
