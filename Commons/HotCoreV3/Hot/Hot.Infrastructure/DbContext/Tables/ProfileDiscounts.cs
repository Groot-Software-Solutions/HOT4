using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Domain.Entities;
using OneOf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Infrastructure.DbContext.Tables
{
    public class ProfileDiscounts : Table<ProfileDiscount>, IProfileDiscounts
    {
        public ProfileDiscounts(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
            base.ListSuffix = "_List";
            base.IdPrefix = "Profile";
        }

        public async Task<OneOf<ProfileDiscount, HotDbException>> DiscountAsync(int ProfileId, int BrandId)
        {
            return await dbHelper.QuerySingle<ProfileDiscount>($"{GetSPPrefix()}_Discount @ProfileId,@BrandId", new { ProfileId, BrandId });
        }
        public OneOf<ProfileDiscount, HotDbException> Discount(int ProfileId, int BrandId)
        {
            return DiscountAsync(ProfileId, BrandId).Result;
        }
    }

}