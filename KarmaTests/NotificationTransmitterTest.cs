// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Karma.Services;
using MatBlazor;
using NUnit.Framework;

namespace KarmaTests
{
    class NotificationTransmitterTest
    {
        private bool eventHandled;
        private NotificationEventArgs eventArgs;

        [SetUp]
        public void SetUp()
        {
            eventHandled = false;
            eventArgs = null;
        }

        [Test, Combinatorial]
        public void ShowMessageTest(
            [Values(null, "", "SingleWord", "Many different words.? !; and stuff")] string message,
            [Values(MatToastType.Info, MatToastType.Danger, MatToastType.Success)] MatToastType type)
        {
            var transmitter = new NotificationTransmitter();
            transmitter.NotificationEvent += HandleEvent;
            transmitter.ShowMessage(message, type);
            Assert.True(eventHandled);
            var expectedEvent = new NotificationEventArgs(message, type);
            Assert.AreEqual(expectedEvent.NotificationType, eventArgs.NotificationType);
            Assert.AreEqual(expectedEvent.Message, eventArgs.Message);
        }

        private void HandleEvent(object sender, NotificationEventArgs e)
        {
            eventHandled = true;
            eventArgs = e;
        }
    }
}
