﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Karma.Services
{
    public class NotificationEventArgs : EventArgs
    {
        public NotificationEventArgs(string message)
        {
            Message = message;
            Time = DateTime.Now;
        }

        public string Message { get; set; }
        public DateTime Time { get; set; }
    }
}
