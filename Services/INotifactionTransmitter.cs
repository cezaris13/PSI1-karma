// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using static Karma.Services.NotificationTransmitter;

namespace Karma.Services
{
    public interface INotifactionTransmitter
    {
        void ShowMessage(string title);
        event NotificationEventHandler NotificationEvent;
    }
}
