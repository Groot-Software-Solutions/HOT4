using Hot.Application.Common.Interfaces;
using Hot.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Infrastructure.Services
{
    public class HotTypeIdentifier : IHotTypeIdentifier
    {
        public HotTypes Identify(string TypeCode, int SplitCount)
        {
            throw new NotImplementedException();
        }

        public HotTypes Identify(string Data)
        {
            throw new NotImplementedException();
        }
    }
}
