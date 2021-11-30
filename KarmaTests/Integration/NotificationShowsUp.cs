// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Karma.Services;
using MatBlazor;
using Moq;
using NUnit.Framework;

namespace KarmaTests.Integration
{
    class NotificationShowsUp
    {
        [Test, Combinatorial]
        public void ShowMessageTest(
            [Values(null, "", "SingleWord", "Many different words.? !; and stuff")] string message,
            [Values(MatToastType.Info, MatToastType.Danger, MatToastType.Success)] MatToastType type)
        {
            var mockToaster = new Mock<IMatToaster>(MockBehavior.Strict);
            var mockToast = new Mock<MatToast>(MockBehavior.Strict, "a", "b", "c", new MatToastOptions(MatToastType.Danger, new MatToastConfiguration()));
            mockToaster
                .Setup(x => x.Add(message, type, It.IsAny<string>(), It.IsAny<string>(), (Action<MatToastOptions>) It.IsAny<object>()))
                .Returns(mockToast.Object);
            var transmitter = new NotificationTransmitter();
            var toaster = new NotificationToaster(mockToaster.Object, transmitter);

            transmitter.ShowMessage(message, type);

            mockToaster.VerifyAll();
        }
    }
}
