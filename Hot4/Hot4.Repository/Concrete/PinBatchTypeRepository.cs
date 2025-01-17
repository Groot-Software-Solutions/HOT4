using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class PinBatchTypeRepository : RepositoryBase<PinBatchTypes>, IPinBatchTypeRepository
    {
        public PinBatchTypeRepository(HotDbContext context) : base(context) { }

        public async Task<bool> AddPinBatchType(PinBatchTypes pinBatchTypes)
        {
            await Create(pinBatchTypes);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdatePinBatchType(PinBatchTypes pinBatchTypes)
        {
            Update(pinBatchTypes);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeletePinBatchType(PinBatchTypes pinBatchTypes)
        {
            Delete(pinBatchTypes);
            await SaveChanges();
            return true;
        }
        public async Task<List<PinBatchTypes>> ListPinBatchType()
        {
            return await GetAll().ToListAsync();
        }
        public async Task<PinBatchTypes?> GetPinBatchTypeById(byte pinBatchTypeId)
        {
            return await GetById(pinBatchTypeId);
        }
    }
}
