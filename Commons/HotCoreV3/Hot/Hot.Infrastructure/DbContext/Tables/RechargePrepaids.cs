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
    public class RechargePrepaids : Table<RechargePrepaid>, IRechargePrepaids
    {
        public RechargePrepaids(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
            base.GetSuffix = "_Select";
            base.AddSuffix = "_Save2"; 
            base.UpdateSuffix = "_Save2";
            base.IdPrefix = "Recharge";

        }

    }

    }
