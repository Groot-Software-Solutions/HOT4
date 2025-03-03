using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IMailService
    {
        public Task<bool> SendSMS(SMS sms);
        public Task HandleMessages(List<Message> messages);
        public List<Message> GetNewEmails();
        public SMS GetSMS(Message message);
         
    }
}
