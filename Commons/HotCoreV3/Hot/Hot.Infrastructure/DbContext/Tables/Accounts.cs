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
    public class Accounts : Table<Account>, IAccounts
    {
        public Accounts(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
            base.GetSuffix = "_Select";
            base.AddSuffix = "_Save";
            base.AddParameters = "@AccountID,@ProfileID,@AccountName,@NationalID,@Email,@ReferredBy";
            base.UpdateSuffix = AddSuffix;
            base.UpdateParameters = AddParameters;
            base.SearchSuffix = "_Search";
            base.SearchParameter = "Filter";
        }

        public Task<OneOf<Account, HotDbException>> SelectRow(long accountId)
        {
            return base.GetAsync((int)accountId);
        }
         
    }
}
