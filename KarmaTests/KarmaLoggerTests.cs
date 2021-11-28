// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Karma.Services;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace KarmaTests
{
    class KarmaLoggerTests
    {
        private string logFilePath = "debug.txt";

        [SetUp]
        public void SetUp()
        {
            DeleteLogFile();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            DeleteLogFile();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("SingleWord")]
        [TestCase("Many different words.? !; and stuff")]
        public void LogInformationTest_SingleMessage(string message)
        {
            var logger = new KarmaLogger();
            logger.LogInformation(message);
            string text = File.ReadAllText(logFilePath);
            Assert.AreEqual($"{LogLevel.Information} - {message}\r\n", text);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("SingleWord")]
        [TestCase("Many different words.? !; and stuff")]
        public void LogInformationTest_MultipleMessages(string message)
        {
            var logger = new KarmaLogger();
            var first = "first line";
            var third = "third line";
            logger.LogInformation(first);
            logger.LogInformation(message);
            logger.LogInformation(third);
            string text = File.ReadAllText(logFilePath);
            var expected = $"Information - {first}\r\n{LogLevel.Information} - {message}\r\nInformation - {third}\r\n";
            Assert.AreEqual(expected, text);
        }

        private void DeleteLogFile()
        {
            File.Delete(logFilePath);
        }
    }
}
