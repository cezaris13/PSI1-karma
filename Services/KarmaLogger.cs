// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using Microsoft.Extensions.Logging;

namespace Karma.Services
{
    public class KarmaLogger : IKarmaLogger
    {
        public void LogInformation(string message)
        {
            string path = "debug.txt";
            if (!File.Exists(path))
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.WriteLine($"{LogLevel.Information} - {message}");
                }
            }
            else
            {
                using (StreamWriter writer = File.AppendText(path))
                {
                    writer.WriteLine($"{LogLevel.Information} - {message}");
                }
            }
        }
    }
}
