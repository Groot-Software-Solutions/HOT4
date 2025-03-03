using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ConfigHelper : IConfigHelper
    {
        private readonly IConfiguration configuration;

        public ConfigHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string CnnVal()
        {
            return CnnVal("DefaultConnection");
        }
        public string CnnVal(string name)
        {
            return configuration.GetConnectionString(name);
        }

        public T GetVal<T>(string name)
        {
            return configuration.GetValue<T>(name);
        }
    }
}
