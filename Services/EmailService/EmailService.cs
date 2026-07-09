using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.EmailService
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("qa3da", _configuration["MailSettings:Email"]));
            email.To.Add(MailboxAddress.Parse(toEmail));

            email.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            try
            {
               await smtp.ConnectAsync(
                    _configuration["MailSettings:Host"],
                    int.Parse(_configuration["MailSettings:Port"]),
                    SecureSocketOptions.StartTls
                );

               await smtp.AuthenticateAsync(
                    _configuration["MailSettings:Email"],
                    _configuration["MailSettings:Password"]
                );

               await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Email Service Error]: {ex}");
                throw;
            }
            finally
            {
                if (smtp.IsConnected)
                    await smtp.DisconnectAsync(true);
            }

        }
    }
}
