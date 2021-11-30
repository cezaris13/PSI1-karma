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
        private readonly ISendGridClient m_sendGridClient;
        public EmailSender(IConfiguration config, ISendGridClient client)
        {
            m_config = config;
            m_sendGridClient = client;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            Func<string, string, EmailAddress> constructFrom = (x, y) => new EmailAddress(x, y);
            return Execute(subject, message, email, constructFrom);
        }

        private Task Execute(string subject, string message, string email, Func<string, string, EmailAddress> constructFrom)
        {
            var msg = new SendGridMessage()
            {
                From = constructFrom(m_config["KarmaEmail"], m_config["PasswordConfirmationName"]),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            msg.SetClickTracking(false, false);
            return m_sendGridClient.SendEmailAsync(msg);
        }
    }
}
