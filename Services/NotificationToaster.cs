// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using MatBlazor;

namespace Karma.Services
{
    public class NotificationToaster
    {
        private IMatToaster m_toaster { get; set; }

        public NotificationToaster(IMatToaster toaster, INotifactionTransmitter transmitter)
        {
            m_toaster = toaster;
            transmitter.NotificationEvent += HandleEvent;
        }

        private delegate void DisplayNotification(string title, MatToastType notificationType);

        private void HandleEvent(Object sender, NotificationEventArgs e)
        {
            DisplayNotification displayNotification = delegate (string title, MatToastType notificationType)
            {
                m_toaster.Add(title, notificationType, configure: config =>
                 {
                     config.MaximumOpacity = Convert.ToInt32(100);

                     config.ShowTransitionDuration = Convert.ToInt32(500);
                     config.VisibleStateDuration = Convert.ToInt32(3000);
                     config.HideTransitionDuration = Convert.ToInt32(500);

                     config.RequireInteraction = false;
                 });
            };

            displayNotification(e.Message, e.NotificationType);
        }
    }
}
