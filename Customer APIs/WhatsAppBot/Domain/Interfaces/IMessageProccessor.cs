using Domain.DataModels;
using Domain.Entities;
using System.Threading.Tasks;

namespace Infrastructure.Handlers
{
    public interface IMessageProccessor
    {
        public Task<WhatsAppTemplate> ProcessMessage(WhatsAppMessage message);
        public SelfTopUp GetSelfTopUp(string message);
        public PinReset GetPinReset(string message, string sender);

    }
}