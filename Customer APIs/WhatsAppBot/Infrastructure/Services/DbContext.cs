using Dapper;
using Domain.DataModels;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class DbContext : IDbContext
    {
        private readonly IDbHelper dbHelper;

        public DbContext(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
          
        public async Task<WhatsAppTemplate> LoadTemplate(TemplateCode templateCode)
        {
            string Query = "spWhatsAppTemplate_Get @templateCode";
            return await dbHelper.QuerySingle<WhatsAppTemplate>(Query, new { TemplateCode= (int)templateCode});
        }

        public async Task<bool> LogData_Save(LogItem logItem)
        {
            string Query = "spLogData_Save @Module, @Method, @Data, @Reference";
            return await dbHelper.Execute(Query, logItem);
        }

        public async Task<bool> WhatsAppLog_Save(WebAPILog whatsAppLog)
        {
            string Query = "spWhatsAppLog_Save @Id,@Module, @Headers, @Method, @Body, @StatusCode, @RequestTime";
            return await dbHelper.Execute(Query, whatsAppLog); 
        }
         

        public async Task<List<WhatsAppMessage>> WhatsAppMessage_Inbox()
        {
            string Query = @"spWhatsAppMessages_Inbox";
            return await dbHelper.Query<WhatsAppMessage>(Query);
        }

        public async Task<int> WhatsAppMessage_Save(WhatsAppMessage whatsAppMessage)
        { 
            string Query = @"spWhatsAppMessage_Save @Id, @Mobile, @Message, @ConversationId, @TypeId, @MessageDate, @StateId";
           return await dbHelper.ExecuteScalar<WhatsAppMessage, int>(Query, whatsAppMessage);
           
        }

    }
}
