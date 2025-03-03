using System.Xml.Linq;

namespace Hot.Application.Common.Extensions
{
    public static class ExceptionExtension
    {
        public static void LogError(this Exception error, ILogger logger)
        {
            var Source = new System.Diagnostics.StackTrace().GetFrame(1)?.GetMethod()?.Name;
            logger.LogError(error, "{ErrorType} occured in {Source} - {Message}", 
                error.GetType().Name, Source, error.Message);
        }

        public static AppException LogAndReturnError(this Exception error, ILogger logger)
        {
            error.LogError(logger);
            return new AppException($"{error.GetType().Name} Error", error);
        }

        public static AppException LogAndReturnError(this Exception error, string Name, ILogger logger)
        {
            error.LogError(logger);
            return new AppException($"{Name} Error", error);
        }

        public static AppException ReturnAppException(this Exception error)
        {
            return new AppException(error.Message, error);
        }

        public static NotFoundException ReturnNotFound(this Exception error, string RequestedItem, string requestedId)
        {
            return new NotFoundException($"{RequestedItem} not found - {RequestedItem}", requestedId);
        }
        public static AccountNotFoundException ReturnAccountNotFound(this Exception error, string RequestedItem, string requestedId)
        {
            return new AccountNotFoundException($"{RequestedItem} not found - {RequestedItem}", requestedId);
        }

        public static FailedToRegisterUserException FailedToRegisterUser(this Exception exception, string accessCode, ILogger logger)
        {
            var error = new FailedToRegisterUserException(accessCode, exception);
            error.LogError(logger);
            return error;
        }

    }
}
