namespace Hot4.Core.Enums;

public enum SmsState : int
{
    Pending = 0,
    Busy = 1,
    Success = 2,
    Failure = 3,
    PendingVerification = 4,
    PendingReversal = 5
}
