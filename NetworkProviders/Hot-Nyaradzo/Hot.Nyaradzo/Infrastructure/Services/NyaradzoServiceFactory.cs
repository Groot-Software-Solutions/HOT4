using Hot.Nyaradzo.Application.Common.Interfaces;
using Hot.Nyaradzo.Application.Common.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Nyaradzo.Infrastructure.Services
{
    public class NyaradzoServiceFactory : INyaradzoServiceFactory
    {
        readonly INyaradzoService MainService;

        public NyaradzoServiceFactory( INyaradzoService nyaradzoService)
        {
            MainService = nyaradzoService; 
        }
        public INyaradzoService GetService()
        {
            return MainService;
        } 
    }
}
