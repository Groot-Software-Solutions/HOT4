using Domain.DataModels;
using Domain.Entities;
using Domain.Enum;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IMessageHandler
    {
        public Task<WhatsAppMessage> HanldeMessage(Message message);
        public RequestType IdentifyRequest(string message); 
    }
}