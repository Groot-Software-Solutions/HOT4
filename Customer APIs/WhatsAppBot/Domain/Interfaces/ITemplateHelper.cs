using Domain.DataModels;
using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITemplateHelper
    {
        public Task<WhatsAppTemplate> LoadTemplateAsync(TemplateCode templateCode);
        public void SetTemplateFields( ref WhatsAppTemplate reply, SelfTopUp recharge);
        public void SetTemplateFields(ref WhatsAppTemplate reply, PinReset pinReset);
    }
}
