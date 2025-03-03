
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Extensions
{
    public static class RechargeServiceCollectionExtensions
    { 
            public static IServiceCollection AddRechargeService(this IServiceCollection collection,
                Action<RechargeServiceOptions> setupAction)
            {
                if (collection == null) throw new ArgumentNullException(nameof(collection));
                if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

                collection.Configure(setupAction);
                return collection;
            }
        
    }
}
