using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Infrastructure.Services
{
   public static class SageServiceCollectionExtensions
    {
        public static IServiceCollection AddSageServiceOptions(this IServiceCollection collection,
               Action<SageServiceOptions> setupAction)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

            collection.Configure(setupAction);
            return collection;
        }

    }
}
