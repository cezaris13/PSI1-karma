// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Karma.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace KarmaTests
{
    public class EmailSenderTests
    {
        [Test]
        public async Task TestSendEmailAsync()
        {
            var sendGridClient = new Mock<ISendGridClient>(MockBehavior.Strict);
            sendGridClient
                .Setup(p => p.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
                .Callback<SendGridMessage, CancellationToken>((message, token) =>
                {
                    Assert.AreEqual("email@gmail.com", message.From.Email);
                    Assert.AreEqual("confirmationName", message.From.Name);
                    Assert.AreEqual("subject", message.Subject);
                    Assert.AreEqual("message", message.PlainTextContent);
                    Assert.AreEqual("message", message.HtmlContent);
                    Assert.AreEqual("test.email@gmail.com", message.Personalizations[0].Tos[0].Email);
                })
                .ReturnsAsync((Response) null);

            var inMemorySettings = new Dictionary<string, string> {
                {"KarmaEmail", "email@gmail.com"},
                {"PasswordConfirmationName", "confirmationName"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var sut = new EmailSender(configuration, sendGridClient.Object);
            await sut.SendEmailAsync("test.email@gmail.com", "subject", "message");

            sendGridClient.VerifyAll();
        }
    }
}
