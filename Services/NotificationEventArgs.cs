// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using MatBlazor;

namespace Karma.Services
{
    public class NotificationEventArgs : EventArgs
    {
        public NotificationEventArgs(string message, MatToastType notificationType)
        {
            Message = message;
            Time = DateTime.Now;
            NotificationType = notificationType;
        }

        public string Message { get; set; }
        public MatToastType NotificationType { get; set; }
        public DateTime Time { get; set; }
    }
}
