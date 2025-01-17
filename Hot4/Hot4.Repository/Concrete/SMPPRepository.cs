
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class SMPPRepository : RepositoryBase<Smpp>, ISMPPRepository
    {
        public SMPPRepository(HotDbContext context) : base(context) { }

        public async Task<bool> AddSMPP(Smpp smpp)
        {
            await Create(smpp);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteSMPP(Smpp smpp)
        {
            Delete(smpp);
            await SaveChanges();
            return true;
        }
        public async Task<List<Smpp>> ListSMPP()
        {
            return await GetAll().ToListAsync();
        }
        public async Task<Smpp?> GetSMPPById(byte smppId)
        {
            return await GetById(smppId);
        }
        public async Task<bool> UpdateSMPP(Smpp smpp)
        {
            Update(smpp);
            await SaveChanges();
            return true;
        }
    }
}
