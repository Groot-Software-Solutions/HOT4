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
            return await _context.Brand.Include(d => d.Network).Where(d => d.BrandId == BrandId)
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
            return await _context.Brand.Include(d => d.Network).Where(d => d.NetworkId == brandIdentitySearchModel.NetworkId
            && d.BrandSuffix == brandIdentitySearchModel.BrandSuffix)
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
            return await _context.Brand.Include(d => d.Network).Select(
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
    }
}
