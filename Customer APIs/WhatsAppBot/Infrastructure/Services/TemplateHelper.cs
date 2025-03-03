using Domain.DataModels;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces; 
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class TemplateHelper : ITemplateHelper
    {
        readonly IDbContext Context;

        public TemplateHelper(IDbContext context)
        {
            Context = context;
        }

        public async Task<WhatsAppTemplate> LoadTemplateAsync(TemplateCode templateCode)
        {
            var template =  await Context.LoadTemplate(templateCode);
            return template;
        }

        public void SetTemplateFields(ref WhatsAppTemplate reply, SelfTopUp recharge)
        {
            reply.Text = reply.Text.Replace("%BillerNumber%", recharge.BillerMobile, StringComparison.OrdinalIgnoreCase);
            reply.Text = reply.Text.Replace("%Amount%", recharge.Amount.ToString("#,#0.00"), StringComparison.OrdinalIgnoreCase);
            reply.Text = reply.Text.Replace("%TargetMobile%", recharge.TargetMobile, StringComparison.OrdinalIgnoreCase);
        }

        public void SetTemplateFields(ref WhatsAppTemplate reply, PinReset pinReset)
        {
            reply.Text = reply.Text.Replace("%IDNumber%", pinReset.IDNumber, StringComparison.OrdinalIgnoreCase);
            reply.Text = reply.Text.Replace("%Name%", pinReset.Names, StringComparison.OrdinalIgnoreCase);
            reply.Text = reply.Text.Replace("%TargetMobile%", pinReset.TargetMobile, StringComparison.OrdinalIgnoreCase);
            reply.Text = reply.Text.Replace("%Sender%",pinReset.Sender, StringComparison.OrdinalIgnoreCase);
        }
    }
}
