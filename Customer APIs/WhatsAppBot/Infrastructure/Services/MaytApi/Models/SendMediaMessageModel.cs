using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Infrastructure.Services.MaytApi.Models
{
    public class SendMediaMessageModel : SendMessageModel
    {
        public string text { get; set; }
    }
}
