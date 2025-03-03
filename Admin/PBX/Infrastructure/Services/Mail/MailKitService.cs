using Application.Common.Interfaces; 
using Domain.Entities;
using Infrastructure.Extensions;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services.Mail
{
    public class MailKitService : Application.Common.Interfaces.IMailService
    {
        readonly MailServiceOptions options;
        readonly PBXOptions pbx;
        readonly IDbContext dbContext;

        public MailKitService(IDbContext dbContext,IOptions<MailServiceOptions> options,  IOptions<PBXOptions> pbx)
        {
            this.dbContext = dbContext;
            this.options = options.Value;
            this.pbx = pbx.Value;
        }

        
        public List<Message> GetNewEmails()
        {
            var result = new List<Message>();
            using (var client = new ImapClient())
            {
                ConnectClient(client);
                var inbox = GetInbox(client);
                var emails = GetPBXEmails(inbox);
                foreach (var uid in emails)
                {
                    ProcessEmail(result, inbox, uid);
                }
                client.Disconnect(true);
            }
            return result;
        } 

        public SMS GetSMS(Message message)
        {
            var result = new SMS
            {
                Mobile = message.Subject.Replace("SMS receive from ", "").Substring(0, 13).Replace("+263","0"),
                Text = message.Body,
                Date = message.Received,
                User = "IMAP-System"
            };

            return result;
        }

        public async Task HandleMessages(List<Message> messages)
        {
            foreach (var item in messages)
            {
                await dbContext
                    .SMSs
                    .AddAsync(GetSMS(item)); 
            } 
        }

        public async Task<bool> SendSMS(SMS sms)
        {
            if (options.AllowedToSend == false) return true;
            try
            {
                MimeMessage message = GetMessage(sms);

                using (var client = new SmtpClient())
                {
                    if (options.SMTPStartTLSEnabled)
                    {
                        client.Connect(options.SMTPHost, options.SMTPPort, SecureSocketOptions.StartTls);
                    }
                    else
                    {
                        client.Connect(options.SMTPHost, options.SMTPPort, options.SMTPTLSEnabled);
                    }

                    if (options.SMTPAuthRequired) client.Authenticate(options.Username, options.Password);

                    await client.SendAsync(message);
                    client.Disconnect(true);
                }
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        private MimeMessage GetMessage(SMS sms)
        {
            var message = new MimeMessage
            {
                Subject = $"port:{pbx.DefaultPort};num:+263{sms.Mobile.TrimStart('0')};code:{pbx.Pin}",
                Body = new TextPart("plain") { Text = sms.Text }
            };
            message.From.Add(new MailboxAddress("Support", options.EmailAddress));
            message.To.Add(new MailboxAddress("PBX", options.EmailAddress));
            return message;
        }

        private void ConnectClient(IImapClient client)
        { 
            // Authenticate the IMAP client: 
            client.Connect(options.IMAPHost, options.IMAPPort, options.IMAPTLSEnabled);
            client.Authenticate(options.Username,options.Password);

        }
        private void ConnectClientO365(IImapClient client)
        {   
            // Get an OAuth token:
            var scopes = new[] { options.ScopeUrl };
            var app = PublicClientApplicationBuilder.Create(options.AppClientId)
                .WithAuthority(AadAuthorityAudience.AzureAdMultipleOrgs)
                .Build();
            var authenticationResult =  app.AcquireTokenByUsernamePassword
                (scopes, options.Username, options.Password).ExecuteAsync().Result;

            // Authenticate the IMAP client: 
             client.Connect(options.IMAPHost, options.IMAPPort, options.IMAPTLSEnabled);
             client.Authenticate(new SaslMechanismOAuth2(options.Username, authenticationResult.AccessToken));
             
        }

        private IMailFolder GetInbox(IImapClient client)
        {
            // var inbox = client.Inbox;
            var inbox = GetSpecificFolder(client, "PBX"); 
            inbox.Open(FolderAccess.ReadWrite);
            return inbox;
        }

        private IList<UniqueId> GetPBXEmails(IMailFolder inbox)
        {
            var query = SearchQuery.SubjectContains("SMS receive from")
                .And(SearchQuery.FromContains(options.EmailAddress))
                .And(SearchQuery.NotSeen);
            return inbox.Search(query);
        }

        private static void ProcessEmail(List<Message> result, IMailFolder inbox, UniqueId uid)
        {
            var message = inbox.GetMessage(uid);
            result.Add(new Message()
            {
                Body = message.TextBody
                , From = message.From.ToString()
                , Received = message.Date.DateTime
                , Subject = message.Subject.ToString()
                , To = message.To.ToString()
            });
            inbox.AddFlags(uid, MessageFlags.Seen, true);
        }

       private static IMailFolder GetSpecificFolder(IImapClient client, string FolderName)
        {
            var personal = client.GetFolder(client.PersonalNamespaces[0]);

            return personal.GetSubfolders(false).First(x => x.Name == FolderName);
        }

    }
}
