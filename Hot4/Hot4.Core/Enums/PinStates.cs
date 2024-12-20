using System;
using System.Collections.Generic;
using System.Text;

namespace Hot4.Core.Enums
{
    public enum PinStates
    {
        Available = 0,
        SoldHotRecharge = 1,
        SoldHotBanking = 2,
        SoldFileExport = 3,
        SoldPromotional = 4,
        DuplicateAvailable = 10,
        DuplicateSold = 11,
        DoNotSell = 12, 
        AvailablePromotional = 13,
    }
}
