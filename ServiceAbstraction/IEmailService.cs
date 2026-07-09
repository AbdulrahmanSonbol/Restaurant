using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceAbstraction
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlMessage);
    }
}
