using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel.ApiModels;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class BrandRepository : RepositoryBase<Brand>, IBrandRepository
    {
        public BrandRepository(HotDbContext context) : base(context) { }

        public async Task<List<BrandModel>> GetBrand(int BrandId)
        {
            return await GetByCondition(d => d.BrandId == BrandId).Include(d => d.Network)
                 .Select(d => new BrandModel
                 {
                     BrandId = d.BrandId,
                     BrandName = d.BrandName,
                     BrandSuffix = d.BrandSuffix,
                     Network = d.Network.Network,
                     NetworkId = d.NetworkId,
                     Prefix = d.Network.Prefix,
                     WalletTypeId = d.WalletTypeId
                 }).ToListAsync();
        }

        public async Task<List<BrandModel>> GetBrandIdentity(BrandIdentitySearchModel brandIdentitySearchModel)
        {
            return await GetByCondition(d => d.NetworkId == brandIdentitySearchModel.NetworkId
            && d.BrandSuffix == brandIdentitySearchModel.BrandSuffix).Include(d => d.Network)
                 .Select(d => new BrandModel
                 {
                     BrandId = d.BrandId,
                     BrandName = d.BrandName,
                     BrandSuffix = d.BrandSuffix,
                     Network = d.Network.Network,
                     NetworkId = d.NetworkId,
                     Prefix = d.Network.Prefix,
                     WalletTypeId = d.WalletTypeId

                 }).ToListAsync();
        }

        public async Task<List<BrandModel>> ListBrand()
        {
            return await GetAll().Include(d => d.Network).Select(
                d => new BrandModel
                {
                    BrandId = d.BrandId,
                    BrandName = d.BrandName,
                    BrandSuffix = d.BrandSuffix,
                    Network = d.Network.Network,
                    NetworkId = d.NetworkId,
                    Prefix = d.Network.Prefix,
                    WalletTypeId = d.WalletTypeId
                }).ToListAsync();
        }
        public async Task AddBrand(Brand brand)
        {
            await Create(brand);
            await SaveChanges();
        }
        public async Task DeleteBrand(Brand brand)
        {
            await Delete(brand);
            await SaveChanges();
        }
        public async Task UpdateBrand(Brand brand)
        {
            await Update(brand);
            await SaveChanges();
        }
    }
}
