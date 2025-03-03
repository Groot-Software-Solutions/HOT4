using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Infrastructure.Extensions
{
    public class PBXOptions
    { 
        public string Pin { get; set; }
        public string DefaultPort { get; set; }
        public List<int> Ports { get; set; }
    }
    public static class PBXOptionsExtensions
    {
        public static IServiceCollection AddPBXServiceOptions(this IServiceCollection collection, Action<PBXOptions> config)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (config == null) throw new ArgumentNullException(nameof(config));

            collection.Configure(config);
            return collection;
        }
         
    }
}
