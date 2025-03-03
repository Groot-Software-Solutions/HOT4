using Domain.DataModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IWhatsAppService
    {
        //public DateTime UnixTimeStampToDateTime(double unixTimeStamp); 
        Task<bool> ReplyAsync(Message message, WhatsAppTemplate reply);
        Message GetMessage<T>(T message);

    }
}
