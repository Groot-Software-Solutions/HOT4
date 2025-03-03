using Domain.DataModels;
using Domain.Enum;
using Domain.Interfaces; 
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WhatsAppTests.Common
{
    public class FakeObjects
    {

        public static Mock<IConfigHelper> GetConfig()
        {
            var config = new Mock<IConfigHelper>();
            config.Setup(s => s.GetVal<string>("AirtimeFormat")).Returns(ConfigHelper.AppSetting("AirtimeFormat"));
            config.Setup(s => s.GetVal<string>("PinResetFormat")).Returns(ConfigHelper.AppSetting("PinResetFormat"));
            config.Setup(s => s.GetVal<string>("HelpMessageFormat")).Returns(ConfigHelper.AppSetting("HelpMessageFormat"));
            config.Setup(s => s.GetVal<string>("HelpEcoCashFormat")).Returns(ConfigHelper.AppSetting("HelpEcoCashFormat"));
            config.Setup(s => s.GetVal<string>("HelpBankingFormat")).Returns(ConfigHelper.AppSetting("HelpBankingFormat"));
            config.Setup(s => s.GetVal<string>("HelpBalanceFormat")).Returns(ConfigHelper.AppSetting("HelpBalanceFormat"));
            config.Setup(s => s.GetVal<string>("HelpRegisterFormat")).Returns(ConfigHelper.AppSetting("HelpRegisterFormat"));
            config.Setup(s => s.GetVal<string>("GreetingFormat")).Returns(ConfigHelper.AppSetting("GreetingFormat"));
            config.Setup(s => s.GetVal<string>("SigningOffFormat")).Returns(ConfigHelper.AppSetting("SigningOffFormat"));
            config.Setup(s => s.GetVal<string>("HelpRechargeFormat")).Returns(ConfigHelper.AppSetting("HelpRechargeFormat"));
            config.Setup(s => s.GetVal<string>("HelpPinFormat")).Returns(ConfigHelper.AppSetting("HelpPinFormat"));

            return config;
        }
     
        public static Mock<IDbContext> GetDBContext()
        {
            var context = new Mock<IDbContext>();
            context.Setup(s => s.WhatsAppMessage_Save(It.IsAny<WhatsAppMessage>())).Returns(Task.FromResult(1));
            context.Setup(s => s.WhatsAppLog_Save(It.IsAny<WebAPILog>())).Returns(Task.FromResult(true));
            context.Setup(s => s.WhatsAppMessage_Inbox()).Returns(Task.FromResult(new List<WhatsAppMessage>()));

            context.Setup(s => s.LoadTemplate(TemplateCode.RechargeAttempt))
                .Returns(Task.FromResult(new WhatsAppTemplate()
                {
                    Id = (int)TemplateCode.RechargeAttempt
                      , Name = "RechargeAttempt"
                      , Text = "Recharge for %TargetMobile% Amount of %Amount% Biller paying %BillerNumber%"
                      , Type = MessageType.Text
                      , ImageData = null
                }));
            context.Setup(s => s.LoadTemplate(TemplateCode.Registration))
                .Returns(Task.FromResult(new WhatsAppTemplate()
                {
                    Id = (int)TemplateCode.RechargeAttempt
                     , Name = "Registration"
                     , Text = "Registration Info"
                     , Type = MessageType.Text
                     , ImageData = null
                }));
            return context;
        }


    }
}
