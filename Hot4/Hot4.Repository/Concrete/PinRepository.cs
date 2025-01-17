using Hot4.Core.Enums;
using Hot4.Core.Settings;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Hot4.Repository.Concrete
{
    public class PinRepository : RepositoryBase<Pins>, IPinRepository
    {
        public ICommonRepository _commonRepository;
        public HotDbContext _context;
        private TemplateSettings _templateSettings { get; }
        private ValueSettings _valueSetting { get; }

        public PinRepository(HotDbContext context, ICommonRepository commonRepository, IOptions<TemplateSettings> templateSetting, IOptions<ValueSettings> valueSetting) : base(context)
        {
            _commonRepository = commonRepository;
            _context = context;
            _templateSettings = templateSetting.Value;
            _valueSetting = valueSetting.Value;
        }
        public async Task<bool> AddPin(Pins pin)
        {
            await Create(pin);
            await SaveChanges();
            return true;
        }
        private async Task<List<Pins>> PinSummary(long pinBatchId, byte pinStateId, List<long> pinIds = null)
        {
            if (pinBatchId > 0)
            {
                return await GetByCondition(d => d.PinBatchId == pinBatchId)
                             .Include(d => d.PinState)
                             .Include(d => d.Brand)
                             .ThenInclude(m => m.Network)
                             .Include(d => d.PinBatch)
                             .ThenInclude(m => m.PinBatchType)
                             .OrderBy(d => d.Brand.BrandName)
                                 .OrderBy(d => d.Pin)
                                .ToListAsync();
            }
            else
             if (pinStateId > 0)
            {
                return await GetByCondition(d => d.PinStateId == pinStateId)
                             .Include(d => d.PinState)
                             .Include(d => d.Brand)
                             .ThenInclude(m => m.Network)
                             .Include(d => d.PinBatch)
                             .ThenInclude(m => m.PinBatchType)
                               .ToListAsync();
            }
            else
             if (pinIds != null && pinIds.Any())
            {
                return await GetByCondition(d => EF.Constant(pinIds).Contains(d.PinId))
                             .Include(d => d.PinState)
                             .Include(d => d.Brand)
                             .ThenInclude(m => m.Network)
                             .Include(d => d.PinBatch)
                             .ThenInclude(m => m.PinBatchType)
                               .ToListAsync();
            }

            return new List<Pins>();

        }
        public async Task<List<Pins>> GetPinDetailByBatchId(long pinBatchId)
        {
            return await PinSummary(pinBatchId, 0);
        }
        public async Task<List<PinLoadedModel>> GetPinLoadedByBatchId(long pinBatchId)
        {
            var result = await PinSummary(pinBatchId, 0);
            if (result != null && result.Any())
            {
                return result.GroupBy(d => new { d.BrandId, d.Brand.BrandName, d.PinValue })
                       .OrderBy(d => d.Key.BrandId).OrderBy(d => d.Key.PinValue)
                       .Select(d => new PinLoadedModel
                       {
                           Stock = d.Count(),
                           PinValue = d.Key.PinValue,
                           BrandId = d.Key.BrandId,
                           BrandName = d.Key.BrandName,
                       }).ToList();
            }

            return new List<PinLoadedModel>();
        }

        public async Task<List<PinLoadedModel>> GetPinStock()
        {
            var result = await PinSummary(0, (int)PinStateType.Available);
            if (result != null && result.Any())
            {
                return result.GroupBy(d => new { d.BrandId, d.Brand.BrandName, d.PinValue })
                       .OrderBy(d => d.Key.BrandId).OrderBy(d => d.Key.PinValue)
                       .Select(d => new PinLoadedModel
                       {
                           Stock = d.Count(),
                           PinValue = d.Key.PinValue,
                           BrandId = d.Key.BrandId,
                           BrandName = d.Key.BrandName,
                       }).ToList();
            }
            return new List<PinLoadedModel>();
        }

        public async Task<List<PinLoadedModel>> GetPinStockPromo()
        {
            var result = await PinSummary(0, (int)PinStateType.AvailablePromotional);
            if (result != null && result.Any())
            {
                return result.GroupBy(d => new { d.BrandId, d.Brand.BrandName, d.PinValue })
                       .OrderBy(d => d.Key.BrandId).OrderBy(d => d.Key.PinValue)
                       .Select(d => new PinLoadedModel
                       {
                           Stock = d.Count(),
                           PinValue = d.Key.PinValue,
                           BrandId = d.Key.BrandId,
                           BrandName = d.Key.BrandName,
                       }).ToList();
            }
            return new List<PinLoadedModel>();
        }
        public async Task<List<Pins>> PinRecharge(PinRechargePayload pinRecharge)
        {
            decimal remainder = pinRecharge.Amount;
            int pinCount = 0;
            var pinList = new List<long>();

            while (remainder > 0)
            {
                var pin = await GetByCondition(p => p.PinStateId == (int)PinStateType.Available
                                && p.BrandId == pinRecharge.BrandId
                                && p.PinValue <= remainder
                                && !EF.Constant(pinList).Contains(p.PinId))
                                .OrderByDescending(p => p.PinValue)
                                .FirstOrDefaultAsync();

                if (pin == null || pin.PinValue == 0)
                    break;

                pinList.Add(pin.PinId);

                pinCount++;
                remainder -= pin.PinValue;
            }

            if (remainder == 0 && pinCount <= 5)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var pinRecords = await _context.Pin.Where(d => EF.Constant(pinList).Contains(d.PinId)).ToListAsync();
                        foreach (var pinIdUsed in pinRecords)
                        {
                            pinIdUsed.PinStateId = (int)PinStateType.SoldHotRecharge;
                        }
                        _context.Pin.UpdateRange(pinRecords);

                        var rechargePins = pinList.Select(pinId => new RechargePin
                        {
                            RechargeId = pinRecharge.RechargeId,
                            PinId = pinId
                        }).ToList();
                        await _context.RechargePin.AddRangeAsync(rechargePins);
                        await _context.SaveChangesAsync();

                        var resultPinSummary = await PinSummary(0, 0, pinList);
                        await transaction.CommitAsync();
                        return resultPinSummary;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            return new List<Pins>();
        }
        public async Task<long> PinRechargePromo(PinRechargePromoPayload pinRechargePromo)
        {
            var access = await (from acss in _context.Access.Include(d => d.Account)
                                where acss.AccessCode == pinRechargePromo.AccessCode
                                join prfDsc in _context.ProfileDiscount.Include(d => d.Brand)
                                on acss.Account.ProfileId equals prfDsc.ProfileId
                                where prfDsc.BrandId == pinRechargePromo.BrandId
                                select new { acss.AccessId, acss.AccountId, acss.Account.ProfileId, prfDsc.Discount, prfDsc.Brand.BrandName })
                                .FirstOrDefaultAsync();

            if (access == null || access?.ProfileId == null || access?.Discount == null || access?.AccountId == null)
                throw new InvalidOperationException(_templateSettings.PinRechargePromoAccessError);

            var AccessId = access.AccessId;
            var ProfileId = access.ProfileId;
            var Discount = access.Discount;
            var AccountId = access.AccountId;
            var BrandName = access.BrandName;
            var accountBalance = await _commonRepository.GetBalance(AccountId);
            var accountSellValue = await _commonRepository.GetSaleValue(AccountId);

            decimal stockSaleValue = (pinRechargePromo.PinValue * pinRechargePromo.Quantity) * ((100 - Discount) / 100);
            string lowFundsResponse = _templateSettings.PinRechargePromoLowFund.Replace("accountBalance", Convert.ToString(accountBalance)).Replace("accountSellValue", Convert.ToString(accountSellValue));
            if (accountBalance < stockSaleValue)
                throw new InvalidOperationException(lowFundsResponse);

            int available = await _context.Pin.CountAsync(p => p.PinStateId == (int)PinStateType.AvailablePromotional
                                   && p.BrandId == pinRechargePromo.BrandId
                                   && p.PinValue == pinRechargePromo.PinValue);

            string stockResponse = _templateSettings.PinRechargePromoStock.Replace("stockAvailable", Convert.ToString(available));
            if (pinRechargePromo.Quantity > available)
                throw new InvalidOperationException(stockResponse);

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var pins = await _context.Pin.Where(p => p.PinStateId == (int)PinStateType.AvailablePromotional
                                     && p.BrandId == pinRechargePromo.BrandId
                                     && p.PinValue == pinRechargePromo.PinValue)
                                    .Take(pinRechargePromo.Quantity)
                                    .ToListAsync();

                    foreach (var pin in pins)
                    {
                        pin.PinStateId = (int)PinStateType.SoldPromotional;
                    }
                    await _context.Pin.AddRangeAsync(pins);
                    await _context.SaveChangesAsync();

                    var recharge = new Recharge
                    {
                        StateId = (int)PinStateType.SoldHotBanking,
                        AccessId = AccessId,
                        Amount = pinRechargePromo.Quantity * pinRechargePromo.PinValue,
                        Discount = Discount,
                        Mobile = pinRechargePromo.Mobile,
                        BrandId = pinRechargePromo.BrandId,
                        RechargeDate = DateTime.Now,
                        InsertDate = DateTime.Now
                    };

                    await _context.Recharge.AddAsync(recharge);
                    await _context.SaveChangesAsync();

                    long rechargeId = recharge.RechargeId;

                    var rechargePins = pins.Select(pin => new RechargePin
                    {
                        RechargeId = rechargeId,
                        PinId = pin.PinId
                    }).ToList();

                    await _context.RechargePin.AddRangeAsync(rechargePins);
                    await _context.SaveChangesAsync();
                    string message = _templateSettings.PinRechargePromoSuccess.Replace("pinRechargePromoFinalAmt", Convert.ToString(pinRechargePromo.Quantity * pinRechargePromo.PinValue)).Replace("pinRechargePromoQty", Convert.ToString(pinRechargePromo.Quantity)).Replace("pinRechargePromoPinValue", Convert.ToString(pinRechargePromo.PinValue)).Replace("pinRechargePromoBrandName", BrandName);
                    var rechargePrepaid = new RechargePrepaid
                    {
                        RechargeId = rechargeId,
                        DebitCredit = false,
                        ReturnCode = "1",
                        Narrative = message,
                        InitialBalance = 0,
                        FinalBalance = 0,
                        InitialWallet = accountBalance,
                        FinalWallet = accountBalance - (pinRechargePromo.Quantity * pinRechargePromo.PinValue),
                        Reference = pinRechargePromo.Mobile + rechargeId.ToString()
                    };

                    await _context.RechargePrepaid.AddAsync(rechargePrepaid);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return rechargeId;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        public async Task<int> PinRedeemedPromo(long accountId)
        {
            var threeDaysAgo = DateTime.Now.AddDays(-3).Date;
            var pinBatchId = _valueSetting.PinRedeemedPromoBatchId;

            return await (from r in _context.Recharge.Include(d => d.Access)
                          where r.Access.AccountId == accountId && r.RechargeDate > threeDaysAgo
                          join rp in _context.RechargePin.Include(d => d.Pin) on r.RechargeId equals rp.RechargeId
                          where rp.Pin.PinBatchId == pinBatchId
                          select r).CountAsync();


        }
        public async Task<List<Pins>> GetPinRechargeByRechId(long rechargeId)
        {
            return await (from p in _context.Pin
                          join rp in _context.RechargePin on p.PinId equals rp.PinId
                          where rp.RechargeId == rechargeId
                          select p)
                          .ToListAsync();
        }
    }
}
