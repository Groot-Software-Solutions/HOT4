using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Interface.Entities
{
    public class Subscriber
    {
        public long SubscriberID { get; set; }
        public long AccountID { get; set; }
        public string SubscriberName { get; set; }
        public string SubscriberMobile { get; set; }
        public Brand Brand { get; set; }
        public bool Active { get; set; }

    }
}
