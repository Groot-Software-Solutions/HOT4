using Hot.API.Client.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Client.Models
{
    public class SubscribersGetResponse :Response
    {
        public List<Subscriber> Subscribers { get; set; }
    }
}
