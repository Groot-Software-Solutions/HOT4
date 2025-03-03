using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Model
{
    public class MobileSMSDetails
    {
        public string  Mobile { get; set; }
        public int NumberofUnreadSMS { get; set; }
        public SMS LastSMS { get; set; }
         
    }
}
