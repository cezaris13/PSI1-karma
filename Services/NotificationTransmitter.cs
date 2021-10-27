// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Karma.Services
{
    public class NotificationTransmitter : INotifactionTransmitter
    {
        private void OnNotificationEvent(NotificationEventArgs args)
        {
            var handler = NotificationEvent;
            handler?.Invoke(this, args);
        }

        //public event NotificationEventHandler NotificationEvent; //Alternative events
        //public delegate void NotificationEventHandler(object sender, NotificationEventArgs e); //Alternative events
        public event EventHandler<NotificationEventArgs> NotificationEvent;

        public void ShowMessage(string message)
        {
            var args = new NotificationEventArgs(message);
            OnNotificationEvent(args);
        }
    }
}
