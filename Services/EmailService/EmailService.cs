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
            var senderEmail = _configuration["MailSettings:Email"] ?? "";
            var host = _configuration["MailSettings:Host"] ?? "";
            var port = int.Parse(_configuration["MailSettings:Port"]!);
            var password = _configuration["MailSettings:Password"] ?? string.Empty;

            email.From.Add(new MailboxAddress("qa3da", senderEmail));
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
                await smtp.ConnectAsync(host, port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(senderEmail, password);
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
