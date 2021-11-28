// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Karma.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace KarmaTests
{
    public class LoggingMiddlewareTests
    {
        [Test]
        public async Task TestMiddlewareInvoke()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["token"] = "tokenValue";
            httpContext.Request.Method = "method";
            httpContext.Request.Path = new PathString("/pathStringValue");
            httpContext.Request.QueryString = new QueryString("?queryStringValue");

            RequestDelegate next = (HttpContext hc) => Task.CompletedTask;

            var karmaLogger = new Mock<IKarmaLogger>(MockBehavior.Strict);
            karmaLogger
                .Setup(p => p.LogInformation(It.IsAny<string>()))
                .Callback<string>(
                (message) =>
                {
                    Assert.AreEqual($"{httpContext.Request.Headers} {httpContext.Request.Body} {httpContext.Request.Method} {httpContext.Request.Path} {httpContext.Request.QueryString}", message);
                })
                .Verifiable();

            var sut = new LoggingMiddleware(next, karmaLogger.Object);
            await sut.Invoke(httpContext);

            karmaLogger.VerifyAll();
        }
    }
}
