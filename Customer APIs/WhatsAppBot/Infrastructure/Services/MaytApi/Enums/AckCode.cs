using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services.MaytApi.Enums
{
    public enum AckCode
    {
        Delivered = 1
        , Reached = 2
        , Seen = 3
        , played = 4
    }
}
