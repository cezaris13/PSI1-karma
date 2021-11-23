// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Karma.Services
{
    public class KarmaLogger : ILogger
    {
        public KarmaLogger()
        {
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new NoopDisposable();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string message = "KarmaLogs: ";
            if (formatter != null)
            {
                message += formatter(state, exception);
            }

            string path = "debug.txt";
            if (!File.Exists(path))
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.WriteLine($"{logLevel} - {eventId.Id} - {message}");
                }
            }
            else
            {
                using (StreamWriter writer = File.AppendText(path))
                {
                    writer.WriteLine($"{logLevel} - {eventId.Id} - {message}");
                }
            }
        }

        private class NoopDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}
