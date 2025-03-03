using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Infrastructure.DbContext.Tables
{
    public class BankTransactionStates : Table<BankTransactionState>, IBankTransactionStates
    {
        public BankTransactionStates(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
        }
    }
}
