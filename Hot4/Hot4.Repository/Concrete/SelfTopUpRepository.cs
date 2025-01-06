using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class SelfTopUpRepository : RepositoryBase<SelfTopUp>, ISelfTopUpRepository
    {
        public SelfTopUpRepository(HotDbContext context) : base(context) { }
        public async Task<long> AddSelfTopUp(SelfTopUp selfTopUp)
        {
            await Create(selfTopUp);
            await SaveChanges();
            return selfTopUp.SelfTopUpId;
        }
        public async Task DeleteSelfTopUp(SelfTopUp selfTopUp)
        {
            Delete(selfTopUp);
            await SaveChanges();
        }
        public async Task<List<SelfTopUpModel>> GetSelfTopUpByBankTrxId(long bankTrxId)
        {
            return await (from s in _context.SelfTopUp.Where(d => d.BankTrxId == bankTrxId)
                          .Include(d => d.Access).Include(d => d.Brand)
                          join st in _context.SelfTopUpState on s.StateId equals st.SelfTopUpStateId
                          select new SelfTopUpModel
                          {
                              BankTrxId = s.BankTrxId,
                              AccessCode = s.Access.AccessCode,
                              AccessId = s.AccessId,
                              Amount = s.Amount,
                              BillerNumber = s.BillerNumber,
                              BrandId = s.BrandId,
                              BrandName = s.Brand.BrandName,
                              Currency = s.Currency,
                              InsertDate = s.InsertDate,
                              NotificationNumber = s.NotificationNumber,
                              ProductCode = s.ProductCode,
                              RechargeId = s.RechargeId,
                              SelfTopUpId = s.SelfTopUpId,
                              SelfTopUpStateName = st.SelfTopUpStateName,
                              StateId = s.StateId,
                              TargetNumber = s.TargetNumber
                          }).OrderByDescending(d => d.SelfTopUpId)
                          .ToListAsync();
        }
        public async Task<List<SelfTopUpModel>> GetSelfTopUpByStateId(byte selfTopUpStateId)
        {
            return await (from s in _context.SelfTopUp.Where(d => d.StateId == selfTopUpStateId)
                 .Include(d => d.Access).Include(d => d.Brand)
                          join st in _context.SelfTopUpState on s.StateId equals st.SelfTopUpStateId
                          select new SelfTopUpModel
                          {
                              BankTrxId = s.BankTrxId,
                              AccessCode = s.Access.AccessCode,
                              AccessId = s.AccessId,
                              Amount = s.Amount,
                              BillerNumber = s.BillerNumber,
                              BrandId = s.BrandId,
                              BrandName = s.Brand.BrandName,
                              Currency = s.Currency,
                              InsertDate = s.InsertDate,
                              NotificationNumber = s.NotificationNumber,
                              ProductCode = s.ProductCode,
                              RechargeId = s.RechargeId,
                              SelfTopUpId = s.SelfTopUpId,
                              SelfTopUpStateName = st.SelfTopUpStateName,
                              StateId = s.StateId,
                              TargetNumber = s.TargetNumber
                          }).OrderBy(d => d.SelfTopUpId)
                          .ToListAsync();
        }
        public async Task UpdateSelfTopUp(SelfTopUp selfTopUp)
        {
            Update(selfTopUp);
            await SaveChanges();
        }
    }
}
