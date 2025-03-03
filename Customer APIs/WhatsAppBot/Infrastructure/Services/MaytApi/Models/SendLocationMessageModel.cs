using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services.MaytApi.Models
{
    public class SendLocationMessageModel : SendMessageModel
    {
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }

    }
}
