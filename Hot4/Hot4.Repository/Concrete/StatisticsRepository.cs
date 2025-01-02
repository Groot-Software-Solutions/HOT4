using Hot4.Core.Enums;
using Hot4.DataModel.Data;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private HotDbContext _context;
        public StatisticsRepository(HotDbContext context)
        {
            _context = context;
        }
        public async Task<List<StatisticsTrafficModel>> GetRechargeTrafficStatistics()
        {
            return await _context.Recharge.Include(d => d.Brand)
                          .ThenInclude(d => d.Network)
                          .Where(d => d.StateId == (int)SmsState.Success
                          && d.RechargeDate > DateTime.Now.AddMinutes(-11))
                          .GroupBy(d => new
                          {
                              Totalminutes = EF.Functions.DateDiffMinute(d.RechargeDate, DateTime.Now), //(int)(DateTime.Now- d.RechargeDate).TotalMinutes,
                              NetworkName = d.Brand.Network.Network
                          })
                         .Select(d => new StatisticsTrafficModel
                         {
                             Name = d.Key.NetworkName,
                             X = d.Key.Totalminutes,
                             Y = d.Count()
                         })
                        .OrderBy(d => d.Name)
                        .ThenByDescending(d => d.X)
                        .ToListAsync();
        }
        public async Task<List<StatisticsTrafficModel>> GetRechargeTrafficDayStatistics()
        {
            return await _context.Recharge.Include(d => d.Brand)
                         .ThenInclude(d => d.Network)
                         .Where(d => d.StateId == (int)SmsState.Success
                         && d.RechargeDate > DateTime.Now.AddHours(-24))
                         .GroupBy(d => new
                         {
                             Hours = d.RechargeDate.Hour,
                             NetworkName = d.Brand.Network.Network
                         })
                        .Select(d => new StatisticsTrafficModel
                        {
                            Name = d.Key.NetworkName,
                            X = d.Key.Hours,
                            Y = d.Count()
                        })
                      .OrderBy(d => d.Name)
                      .ThenByDescending(d => d.X)
                      .ToListAsync();
        }
        public async Task<List<StatisticsTrafficModel>> GetSMSTrafficStatistics()
        {
            return await _context.Sms.Include(d => d.State)
                         .Where(d => d.Direction == true
                         && d.Smsdate > DateTime.Now.AddMinutes(-11))
                         .GroupBy(d => new { d.Smsdate.Minute, d.Direction, d.State.State })
                         .Select(d => new StatisticsTrafficModel
                         {
                             Name = d.Key.State,
                             X = d.Key.Minute,
                             Y = d.Count()

                         })
                        .OrderBy(d => d.Name)
                        .ThenBy(d => d.Name)
                        .ToListAsync();
        }
    }
}
