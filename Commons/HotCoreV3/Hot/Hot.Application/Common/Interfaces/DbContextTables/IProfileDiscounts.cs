namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IProfileDiscounts : IDbContextTable<ProfileDiscount>
        , IDbCanList<ProfileDiscount>
    {
        public Task<OneOf<ProfileDiscount, HotDbException>> DiscountAsync(int ProfileId, int BrandId);
    }
}
