using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Domain.Enums;

public enum States : int
{
    Pending = 0,
    Busy = 1,
    Success = 2,
    Failure = 3,
    PendingVerification = 4
}
