using Microsoft.AspNetCore.Mvc;

namespace Hot.Tests.Common.Extensions
{
    public static class ActionResultTestExtensions
    {
        
        public static T? GetOkObjectResultValue<T>(this ActionResult<T> result)
        {
            var obj = result.Result;
            if (obj == null) return default;    
            return (T?)((OkObjectResult)obj).Value ;
        }

        public static int? GetOkObjectResultStatusCode<T>(this ActionResult<T> result)
        {
            var obj = result.Result;
            if (obj == null) return default;
            return ((OkObjectResult)obj).StatusCode;
        }

        public static int? GetNotFoundResultStatusCode<T>(this ActionResult<T> result)
        {

            var obj = result.Result;
            if (obj == null) return default;
            return ((NotFoundObjectResult)obj).StatusCode;
        }
    }
}
