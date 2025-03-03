using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hot.Web.Provider.Interfaces.Models
{
    public class BaseResponse
    {
        public int ReplyCode { get; set; }
        public string ReplyMessage { get; set; }
        public string Reference { get; set; }

    }
}