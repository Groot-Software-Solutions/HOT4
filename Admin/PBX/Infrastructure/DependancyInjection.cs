using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Extensions;
using Infrastructure.Services;
using Infrastructure.Services.DbContext;
using Infrastructure.Services.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IConfigHelper, ConfigHelper>();
            services.AddSingleton<IDbHelper, DbHelper>(); 
            services.AddSingleton<IDbContextTable<SMS>, SMSTable>();
            services.AddSingleton<IDbContext, DbContext>();
           
            services.AddOptions();
            
            services.AddPBXServiceOptions(options =>
            {
                var section = configuration.GetSection("PBX").Get<PBXOptions>();
                options.DefaultPort = section.DefaultPort;
                options.Ports = section.Ports;
                options.Pin = section.Pin;
            });


            services.AddMailSereviceOptions(options =>
            {
                var section = configuration.GetSection("Mail").Get<MailServiceOptions>();
                options.AllowedToSend = section.AllowedToSend;
                options.EmailAddress = section.EmailAddress; 
                options.Username = section.Username;
                options.Password = section.Password;
                options.IMAPHost = section.IMAPHost;
                options.IMAPPort = section.IMAPPort;
                options.IMAPTLSEnabled = section.IMAPTLSEnabled; 
                options.POP3Host = section.POP3Host;
                options.POP3Port = section.POP3Port;
                options.POP3TLSEnabled = section.POP3TLSEnabled;
                options.SMTPAuthRequired = section.SMTPAuthRequired;
                options.SMTPHost = section.SMTPHost;
                options.SMTPPort = section.SMTPPort;
                options.SMTPStartTLSEnabled = section.SMTPStartTLSEnabled;
                options.SMTPTLSEnabled = section.SMTPTLSEnabled;
                options.AppClientId = section.AppClientId;
                options.ScopeUrl = section.ScopeUrl;


            });

            services.AddSingleton<IMailService, MailKitService>();

            return services;
        }

    }
}
