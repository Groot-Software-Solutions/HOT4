using Domain.DataModels;
using Infrastructure;
using Infrastructure.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class App
    {
        readonly IMessageProccessor messageProccessor;

        public App(IMessageProccessor messageProccessor)
        {
            this.messageProccessor = messageProccessor;
        }

        public void Run()
        {
            Console.WriteLine("App Started");
            ServiceLoop();
            Console.WriteLine("App Loop Completed. Press any key to quit");
            Console.ReadKey();     
        }

        private void ServiceLoop()
        {
            //ReadDBForNewMesages
            //Iterate through Messages
            //SendMessage to Processing 
                //Reply to Cutstomer

            //debug stuff
            DebugStuff();

        }

        private void DebugStuff()
        {
            var message = new WhatsAppMessage()
            {
                Id = 5,
                ConversationId = "1",
                Message = "Airtime 2 0772397464 0772397464",
                MessageDate = DateTime.Now,
                Mobile = "0772397464",
                StateId = 0,
                TypeId = Domain.Enum.RequestType.Airtime
            };  
            var reply =  messageProccessor.ProcessMessage(message).GetAwaiter().GetResult();

            Console.WriteLine(reply.Text);
        }
    }
}
