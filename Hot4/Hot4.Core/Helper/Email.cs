﻿using System.Net;
using System.Net.Mail;

namespace Hot4.Core.Helper
{
    public static class Email
    {
        public static void SendEmail(string recipient, string subject, string body, string[] cc = null)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp_server")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("email@example.com", "password"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("email@example.com"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(recipient);
                if (cc != null && cc.Count() > 0)
                {
                    foreach (string cemail in cc)
                    {
                        mailMessage.CC.Add(cemail);
                    }

                }

                smtpClient.Send(mailMessage);
            }
            catch
            {

            }
        }
    }
}
