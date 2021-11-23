// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using MatBlazor;

namespace Karma.Services
{
    public class NotificationTransmitter : INotificationTransmitter
    {
        private void OnNotificationEvent(NotificationEventArgs args)
        {
            NotificationEvent(this, args);
        }

        public event EventHandler<NotificationEventArgs> NotificationEvent;

        public void ShowMessage(string message, MatToastType notificationType)
        {
            var args = new NotificationEventArgs(message,notificationType);
            OnNotificationEvent(args);
        }
    }
}
