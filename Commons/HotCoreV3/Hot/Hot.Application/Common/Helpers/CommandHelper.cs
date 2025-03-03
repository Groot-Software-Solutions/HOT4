using System.Data;
using System.Runtime.CompilerServices;

namespace Hot.Application.Common.Helpers
{
    public static class CommandHelper
    {

        public static AppException RollbackAndReturnAppError(Tuple<IDbConnection, IDbTransaction> dbtransaction, IDbHelper DbHelper, [CallerMemberName] string callerMember = "")
        {
            DbHelper.RollBackTransaction(dbtransaction.Item2);
            return new AppException(callerMember, "Transaction error Rollback initiated.");
        }

        public static AppException ReturnDbException( this HotDbException exception, ILogger logger)
        { 
            return exception.LogAndReturnError("Database Error",logger);
        }
    }
}
