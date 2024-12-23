﻿using Hot4.Core.Enums;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel.ApiModels;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class PinRepository : RepositoryBase<Pins>, IPinRepository
    {
        public ICommonRepository _commonRepository;
        public PinRepository(HotDbContext context, ICommonRepository commonRepository) : base(context)
        {
            _commonRepository = commonRepository;
        }
        public async Task<long> AddPin(Pins pin)
        {
            await Create(pin);
            await SaveChanges();
            return pin.PinId;
        }
        private async Task<List<PinDetailModel>> PinSummary(long pinBatchId, byte pinStateId, List<long> pinIds = null)
        {
            if (pinBatchId > 0)
            {
                return await GetByCondition(d => d.PinBatchId == pinBatchId)
                      .Include(d => d.PinState).Include(d => d.Brand).ThenInclude(m => m.Network)
                      .Include(d => d.PinBatch).ThenInclude(m => m.PinBatchType)
                                 .Select(d => new PinDetailModel
                                 {
                                     PinId = d.PinId,
                                     Pin = d.Pin,
                                     PinNumber = d.Pin,
                                     PinRef = d.PinRef,
                                     PinValue = d.PinValue,
                                     PinExpiry = d.PinExpiry,
                                     PinBatchId = d.PinBatchId,
                                     PinBatch = d.PinBatch.PinBatch,
                                     BatchDate = d.PinBatch.BatchDate,
                                     PinStateId = d.PinStateId,
                                     PinState = d.PinState.PinState,
                                     BrandId = d.BrandId,
                                     BrandName = d.Brand.BrandName,
                                     BrandSuffix = d.Brand.BrandSuffix,
                                     PinBatchTypeId = d.PinBatch.PinBatchTypeId,
                                     PinBatchType = d.PinBatch.PinBatchType.PinBatchType,
                                     NetworkId = d.Brand.NetworkId,
                                     Network = d.Brand.Network.Network,
                                     Prefix = d.Brand.Network.Prefix
                                 }).ToListAsync();
            }
            else
             if (pinStateId > 0)
            {
                return await GetByCondition(d => d.PinStateId == pinStateId)
                     .Include(d => d.PinState).Include(d => d.Brand).ThenInclude(m => m.Network)
                     .Include(d => d.PinBatch).ThenInclude(m => m.PinBatchType)
                                .Select(d => new PinDetailModel
                                {
                                    PinId = d.PinId,
                                    Pin = d.Pin,
                                    PinNumber = d.Pin,
                                    PinRef = d.PinRef,
                                    PinValue = d.PinValue,
                                    PinExpiry = d.PinExpiry,
                                    PinBatchId = d.PinBatchId,
                                    PinBatch = d.PinBatch.PinBatch,
                                    BatchDate = d.PinBatch.BatchDate,
                                    PinStateId = d.PinStateId,
                                    PinState = d.PinState.PinState,
                                    BrandId = d.BrandId,
                                    BrandName = d.Brand.BrandName,
                                    BrandSuffix = d.Brand.BrandSuffix,
                                    PinBatchTypeId = d.PinBatch.PinBatchTypeId,
                                    PinBatchType = d.PinBatch.PinBatchType.PinBatchType,
                                    NetworkId = d.Brand.NetworkId,
                                    Network = d.Brand.Network.Network,
                                    Prefix = d.Brand.Network.Prefix
                                }).ToListAsync();
            }
            else
             if (pinIds != null && pinIds.Count > 0)
            {
                return await GetByCondition(d => pinIds.Contains(d.PinId))
                     .Include(d => d.PinState).Include(d => d.Brand).ThenInclude(m => m.Network)
                     .Include(d => d.PinBatch).ThenInclude(m => m.PinBatchType)
                                .Select(d => new PinDetailModel
                                {
                                    PinId = d.PinId,
                                    Pin = d.Pin,
                                    PinNumber = d.Pin,
                                    PinRef = d.PinRef,
                                    PinValue = d.PinValue,
                                    PinExpiry = d.PinExpiry,
                                    PinBatchId = d.PinBatchId,
                                    PinBatch = d.PinBatch.PinBatch,
                                    BatchDate = d.PinBatch.BatchDate,
                                    PinStateId = d.PinStateId,
                                    PinState = d.PinState.PinState,
                                    BrandId = d.BrandId,
                                    BrandName = d.Brand.BrandName,
                                    BrandSuffix = d.Brand.BrandSuffix,
                                    PinBatchTypeId = d.PinBatch.PinBatchTypeId,
                                    PinBatchType = d.PinBatch.PinBatchType.PinBatchType,
                                    NetworkId = d.Brand.NetworkId,
                                    Network = d.Brand.Network.Network,
                                    Prefix = d.Brand.Network.Prefix
                                }).ToListAsync();
            }
            else
            {
                return new List<PinDetailModel>();
            }
        }
        public async Task<List<PinDetailModel>> GetPinDetail_by_batchId(long pinBatchId)
        {
            return await PinSummary(pinBatchId, 0);
        }
        public async Task<List<PinLoadedModel>> GetPinLoaded_by_batchId(long pinBatchId)
        {
            var result = await PinSummary(pinBatchId, 0);
            if (result != null && result.Count > 0)
            {
                return result.GroupBy(d => new { d.BrandId, d.BrandName, d.PinValue })
                       .OrderBy(d => d.Key.BrandId).OrderBy(d => d.Key.PinValue)
                       .Select(d => new PinLoadedModel
                       {
                           Stock = d.Count(),
                           PinValue = d.Key.PinValue,
                           BrandId = d.Key.BrandId,
                           BrandName = d.Key.BrandName,
                       }).ToList();
            }
            else
            {
                return new List<PinLoadedModel>();
            }
        }

        public async Task<List<PinLoadedModel>> GetPinStock()
        {
            var result = await PinSummary(0, (int)PinStateType.Available);
            if (result != null && result.Count > 0)
            {
                return result.GroupBy(d => new { d.BrandId, d.BrandName, d.PinValue })
                       .OrderBy(d => d.Key.BrandId).OrderBy(d => d.Key.PinValue)
                       .Select(d => new PinLoadedModel
                       {
                           Stock = d.Count(),
                           PinValue = d.Key.PinValue,
                           BrandId = d.Key.BrandId,
                           BrandName = d.Key.BrandName,
                       }).ToList();
            }
            else
            {
                return new List<PinLoadedModel>();
            }
        }

        public async Task<List<PinLoadedModel>> GetPinStockPromo()
        {
            var result = await PinSummary(0, (int)PinStateType.AvailablePromotional);
            if (result != null && result.Count > 0)
            {
                return result.GroupBy(d => new { d.BrandId, d.BrandName, d.PinValue })
                       .OrderBy(d => d.Key.BrandId).OrderBy(d => d.Key.PinValue)
                       .Select(d => new PinLoadedModel
                       {
                           Stock = d.Count(),
                           PinValue = d.Key.PinValue,
                           BrandId = d.Key.BrandId,
                           BrandName = d.Key.BrandName,
                       }).ToList();
            }
            else
            {
                return new List<PinLoadedModel>();
            }
        }
        public async Task<List<PinDetailModel>> PinRecharge(PinRechargePayload pinRecharge)
        {
            decimal remainder = pinRecharge.Amount;
            int pinCount = 0;
            var pinList = new List<long>();

            while (remainder > 0)
            {
                var pin = await GetByCondition(p => p.PinStateId == (int)PinStateType.Available
                  && p.BrandId == pinRecharge.BrandId
                  && p.PinValue <= remainder
                  && !pinList.Contains(p.PinId))
                     .OrderByDescending(p => p.PinValue).FirstOrDefaultAsync();

                if (pin == null || pin.PinValue == 0)
                    break;

                pinList.Add(pin.PinId);

                pinCount++;
                remainder -= pin.PinValue;
            }
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (remainder == 0 && pinCount <= 5)
                    {
                        foreach (var pinIdUsed in pinList)
                        {
                            var pinToUpdate = await _context.Pin.FirstOrDefaultAsync(p => p.PinId == pinIdUsed);
                            if (pinToUpdate != null)
                            {
                                pinToUpdate.PinStateId = (int)PinStateType.SoldHotRecharge;
                            }

                            await _context.RechargePin.AddAsync(new RechargePin
                            {
                                RechargeId = pinRecharge.RechargeId,
                                PinId = pinIdUsed
                            });
                        }
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return await PinSummary(0, 0, pinList);
                    }
                    else
                    {
                        return new List<PinDetailModel>();
                    }
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        public async Task<long> PinRechargePromo(PinRechargePromoPayload pinRechargePromo)
        {
            var access = await (from acss in _context.Access.Include(d => d.Account)
                                join prfDsc in _context.ProfileDiscount.Include(d => d.Brand) on acss.Account.ProfileId equals prfDsc.ProfileId
                                where prfDsc.BrandId == pinRechargePromo.BrandId && acss.AccessCode == pinRechargePromo.AccessCode
                                select new { acss.AccessId, acss.AccountId, acss.Account.ProfileId, prfDsc.Discount, prfDsc.Brand.BrandName }).FirstOrDefaultAsync();


            if (access == null || access?.ProfileId == null || access?.Discount == null || access?.AccountId == null)
                throw new InvalidOperationException("This Access Code may not sell this Pin Brand");

            var AccessId = access.AccessId;
            var ProfileId = access.ProfileId;
            var Discount = access.Discount;
            var AccountId = access.AccountId;
            var BrandName = access.BrandName;
            var accountBalance = await _commonRepository.GetBalance(AccountId);
            var accountSellValue = await _commonRepository.GetSaleValue(AccountId);

            decimal stockSaleValue = (pinRechargePromo.PinValue * pinRechargePromo.Quantity) * ((100 - Discount) / 100);
            string lowFundsResponse = $"Your pin purchase failed due to insufficient balance. Your balance is US$ {accountBalance}. You can sell approximately US$ {accountSellValue}";

            if (accountBalance < stockSaleValue)
                throw new InvalidOperationException(lowFundsResponse);

            int available = await _context.Pin.CountAsync(p => p.PinStateId == (int)PinStateType.AvailablePromotional && p.BrandId == pinRechargePromo.BrandId
                                            && p.PinValue == pinRechargePromo.PinValue);


            string stockResponse = $"We have received your Bulk EVD request but do not have correct stock to process it. Stock quantity unavailable ({available} in stock)";

            if (pinRechargePromo.Quantity > available)
                throw new InvalidOperationException(stockResponse);

            // Begin Transaction
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Update Pin State and collect inserted PinIds
                    var pins = await _context.Pin
                        .Where(p => p.PinStateId == (int)PinStateType.AvailablePromotional && p.BrandId == pinRechargePromo.BrandId && p.PinValue == pinRechargePromo.PinValue)
                        .Take(pinRechargePromo.Quantity).ToListAsync();


                    foreach (var pin in pins)
                    {
                        pin.PinStateId = (int)PinStateType.SoldPromotional;
                    }
                    await _context.SaveChangesAsync();

                    // Create TblRecharge entry
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

                    // Insert into TblRechargePin
                    var rechargePins = pins.Select(pin => new RechargePin
                    {
                        RechargeId = rechargeId,
                        PinId = pin.PinId
                    }).ToList();

                    await _context.RechargePin.AddRangeAsync(rechargePins);
                    await _context.SaveChangesAsync();

                    // Create message for TblRechargePrepaid
                    string message = $"We have fulfilled a Promo Pin Purchase of ${pinRechargePromo.Quantity * pinRechargePromo.PinValue} for <br/> Quantity: {pinRechargePromo.Quantity} EVD Pins <br/> Pin Denomination: {pinRechargePromo.PinValue} <br/> Brand: {BrandName}";

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
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public async Task<PinRedeemedPromoModel> PinRedeemedPromo(long accountId)
        {
            var model = new PinRedeemedPromoModel();
            var threeDaysAgo = DateTime.Now.AddDays(-3).Date;
            var pinBatchId = 10008465;

            var transactions = await (from r in _context.Recharge.Include(d => d.Access)
                                      join rp in _context.RechargePin.Include(d => d.Pin) on r.RechargeId equals rp.RechargeId
                                      where r.Access.AccountId == accountId
                                      && rp.Pin.PinBatchId == pinBatchId
                                      && r.RechargeDate > threeDaysAgo
                                      select r).CountAsync();
            if (transactions > 0)
            {
                return new PinRedeemedPromoModel
                {
                    HasPurchased = true
                };
            }
            else
            {
                return new PinRedeemedPromoModel
                {
                    HasPurchased = false
                };
            }

        }
    }
}
