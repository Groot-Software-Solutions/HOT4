namespace Sage.Domain.Enums
{
    public enum HTMLResponseCodes : int
    {
        OK = 200,
        Created = 201,
        Processing = 202,
        Bad_Request = 400,
        Authentication_Failure = 401,
        Bad_System_Request = 402,
        Forbidden = 403,
        Not_Found = 404,
        Method_Not_Supported = 405,
        Server_Busy_Unavailable = 503
    }



}
