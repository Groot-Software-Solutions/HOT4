using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class ReplytoSMSRequest
    { 
        public int ReplyToId { get; set; }
        public SMS Reply { get; set; }
    }
}
