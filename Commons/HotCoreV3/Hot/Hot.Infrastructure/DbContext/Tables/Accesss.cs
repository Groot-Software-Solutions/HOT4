using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Domain.Entities;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Infrastructure.DbContext.Tables
{
    public class Accesss : Table<Access>, IAccesss
    {
        public Accesss(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
            base.GetSuffix = "_Select";
            base.AddSuffix = "_Save2";
            base.AddParameters = "@AccessID,@AccountID,@ChannelID,@AccessCode,@PasswordHash,@PasswordSalt,@AccessPassword";
            base.UpdateSuffix = AddSuffix;
            base.UpdateParameters = AddParameters;
            base.SearchSuffix = "_SelectCode";
            base.SearchParameter = "AccessCode";

        }

        public async Task<OneOf<long, HotDbException>> AdminSelectAsync(long accountId)
        {
            return await dbHelper.QuerySingle<long>($"{GetSPPrefix()}_Admin_Select @accountId", new { accountId });
        }

        public async Task<OneOf<List<Access>, HotDbException>> ListActiveAsync(long accountId)
        {
            return await dbHelper.Query<Access>($"{GetSPPrefix()}_List @accountId", new { accountId });
        }

        public async Task<OneOf<List<Access>, HotDbException>> ListAsync(long accountId)
        {
            return await dbHelper.Query<Access>($"{GetSPPrefix()}_ListAll @accountId", new { accountId });
        }

        public async Task<OneOf<List<Access>, HotDbException>> ListDeletedAsync(long accountId)
        {  
            return await dbHelper.Query<Access>($"{GetSPPrefix()}_ListDeleted  @accountId", new { accountId });
        }

        public async Task<OneOf<bool, HotDbException>> PasswordChangeAsync(Access access)
        {
            return await dbHelper.Execute($"{GetSPPrefix()}_PasswordChange @AccessId,@NewPassword", new { access.AccessId, NewPassword = access.AccessPassword });
        }

        public async Task<OneOf<Access, HotDbException>> SelectCodeAsync(string accessCode)
        {
            return await dbHelper.QuerySingle<Access>($"{GetSPPrefix()}_SelectCode  @accessCode", new { accessCode }); 
        }

        public async Task<OneOf<Access, HotDbException>> SelectLoginAsync(string accessCode, string accessPassword)
        {
            return await dbHelper.QuerySingle<Access>($"{GetSPPrefix()}_SelectLogin  @accessCode, @accessPassword", new { accessCode, accessPassword });
        }

        public async Task<OneOf<Access, HotDbException>> SelectRowAsync(long accessId)
        {
            return await GetAsync((int)accessId);
        }

        public async Task<OneOf<bool, HotDbException>> UnDeleteAsync(long accessId)
        {
            return await dbHelper.Execute($"{GetSPPrefix()}_UnDelete @AccessId", new { accessId });
        }

        public OneOf<bool, HotDbException> PasswordChange(Access access)
            => PasswordChangeAsync(access).Result;

        public OneOf<long, HotDbException> AdminSelect(long accountId)
            => AdminSelectAsync(accountId).Result;

        public OneOf<Access, HotDbException> SelectCode(string accessCode)
            => SelectCodeAsync(accessCode).Result;

        public OneOf<Access, HotDbException> SelectLogin(string accessCode, string accessPassword)
            => SelectLoginAsync(accessCode, accessPassword).Result;

        public OneOf<Access, HotDbException> SelectRow(long accessId)
            => SelectRowAsync(accessId).Result;

        public OneOf<bool, HotDbException> UnDelete(long accessId)
            => UnDeleteAsync(accessId).Result;

        public OneOf<List<Access>, HotDbException> ListDeleted(long accountId)
            => ListDeletedAsync(accountId).Result;

        public OneOf<List<Access>, HotDbException> ListActive(long AccountId)
            => ListActiveAsync(AccountId).Result;

        public OneOf<List<Access>, HotDbException> List(long AccountId)
           => ListAsync(AccountId).Result;

    }
}
