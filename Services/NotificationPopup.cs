// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using MatBlazor;

namespace Karma.Services
{
    public class NotificationPopup : INotificationPopup
    {
        private IMatToaster m_toaster { get; set; }

        public NotificationPopup(IMatToaster toaster)
        {
            m_toaster = toaster;
        }

        public void ShowMessage(string title)
        {
            m_toaster.Add(title, MatToastType.Success, "", "", config =>
                {
                    config.MaximumOpacity = Convert.ToInt32(100);

                    config.ShowTransitionDuration = Convert.ToInt32(500);
                    config.VisibleStateDuration = Convert.ToInt32(3000);
                    config.HideTransitionDuration = Convert.ToInt32(500);

                    config.RequireInteraction = false;
                });

        }
    }
}
