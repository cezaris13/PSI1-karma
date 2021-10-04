using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
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
            return Execute(m_config["SendGridApiKey"], subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(m_config["KarmaEmail"], m_config["PasswordConfirmationName"]),
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
