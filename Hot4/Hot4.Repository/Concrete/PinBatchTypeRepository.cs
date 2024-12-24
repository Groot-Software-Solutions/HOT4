using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel.ApiModels;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class PinBatchTypeRepository : RepositoryBase<PinBatchTypes>, IPinBatchTypeRepository
    {
        public PinBatchTypeRepository(HotDbContext context) : base(context) { }
        public async Task<List<PinBatchTypeModel>> ListPinBatchType()
        {
            return await GetAll()
                .Select(d => new PinBatchTypeModel
                {
                    PinBatchTypeId = d.PinBatchTypeId,
                    PinBatchType = d.PinBatchType,
                }).ToListAsync();
        }
    }
}
