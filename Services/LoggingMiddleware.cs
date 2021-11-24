// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Karma.Services
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate m_next;
        private readonly IKarmaLogger m_logger;

        public LoggingMiddleware(RequestDelegate next, IKarmaLogger logger)
        {
            m_next = next;
            m_logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            m_logger.LogInformation(FormatRequest(context.Request));
            await m_next(context);
        }

        private string FormatRequest(HttpRequest request)
        {
            return $"{request.Headers} {request.Body} {request.Method} {request.Path} {request.QueryString}";
        }
    }
}
