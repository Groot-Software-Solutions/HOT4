using Application.Actions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class App
    {
        private const int CheckforMailTimer = 5000;
        readonly IMediator mediator;

        public App(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public void Run()
        {
            try
            {
                Console.WriteLine("App Started");
                ServiceLoopAsync().GetAwaiter().GetResult();
                Console.WriteLine("App Loop Completed. Press any key to quit");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task ServiceLoopAsync()
        {
            while (true)
            {
                Task.Delay(CheckforMailTimer).GetAwaiter().GetResult();
                await mediator
                .Send(new CheckForNewSMSs())
                .ConfigureAwait(true);
                Console.WriteLine($"Checked for New Messages - {DateTime.Now:dd MMM yy - HH:mm:ss}");
            } 
        }
    }
}
