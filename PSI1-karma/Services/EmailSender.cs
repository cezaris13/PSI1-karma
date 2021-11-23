using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Karma.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration m_config;

        public EmailSender(IConfiguration config)
        {
            m_config = config;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            Func<string, string, EmailAddress> constructFrom = (x, y) => new EmailAddress(x, y);
            return Execute(m_config["SendGridApiKey"], subject, message, email, constructFrom);
        }

        public Task Execute(string apiKey, string subject, string message, string email, Func<string, string, EmailAddress> constructFrom)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = constructFrom(m_config["KarmaEmail"], m_config["PasswordConfirmationName"]),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            msg.SetClickTracking(false, false);
            return client.SendEmailAsync(msg);
        }
    }
}
