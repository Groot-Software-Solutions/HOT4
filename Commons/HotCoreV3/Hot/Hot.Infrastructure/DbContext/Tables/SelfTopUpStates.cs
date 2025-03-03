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
    public class SelfTopUpStates : Table<SelfTopUpState>, ISelfTopUpStates
    {
        public SelfTopUpStates(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
            base.IdPrefix = "SelfTopUpState";
            base.AddSuffix = "_Save";
            base.AddParameters = "@SelfTopUpStateId,@SelfTopUpStateName";
            base.UpdateSuffix = AddSuffix;
            base.UpdateParameters = AddParameters;
            base.GetSuffix = "_Get";

        }

    
       
    }
}
