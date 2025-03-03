using BillPayments.BackgroundTasks.Data;
using BillPayments.BackgroundTasks.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillPayments.BackgroundTasks
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddBackgroundTasks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IBackgroundTaskService, BackgroundTaskService>();

            return services;

        }
    }
}
