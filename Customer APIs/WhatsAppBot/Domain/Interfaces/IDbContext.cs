using Domain.DataModels;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDbContext
    {
        public Task<bool> WhatsAppLog_Save(WebAPILog whatsAppLog);
        public Task<int> WhatsAppMessage_Save(WhatsAppMessage whatsAppMessage);
        public Task<WhatsAppTemplate> LoadTemplate(TemplateCode templateCode);
        public Task<List<WhatsAppMessage>> WhatsAppMessage_Inbox();
        public Task<bool> LogData_Save(LogItem logItem);

    }
}
