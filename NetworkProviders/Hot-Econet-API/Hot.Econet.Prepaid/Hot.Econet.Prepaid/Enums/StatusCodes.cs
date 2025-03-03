namespace Hot.Econet.Prepaid.Enums;

public enum StatusCodes
{
    Success = 0,
    Invalid_Account = 901,
    Invalid_Provider = 902,
    Invalid_Product = 903,
    Invalid_Bundle_Quantity = 904,
    Insufficient_Credit = 905,
    Limits_Exceeded = 906,
    Invalid_Amount = 907,
    Invalid_Voucher = 911,
    Invalid_Batch = 921,
    Invalid_Specification = 922,
    Download_Encryption_Error = 923,
    Invalid_Batch_State = 924,
    Invalid_User = 970,
    Invalid_PIN_or_Password = 971,
    Username_already_exists = 972,
    Request_Timeout = 990,
    General_Error = 999
}
