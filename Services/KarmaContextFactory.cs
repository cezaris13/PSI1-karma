// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Karma.Models;

namespace Karma.Services
{
    public class KarmaContextFactory : IKarmaContextFactory
    {
        private readonly Lazy<KarmaContext> m_karmaContext;
        public KarmaContextFactory()
        {
            m_karmaContext = new Lazy<KarmaContext>(() => new KarmaContext());
        }
        public KarmaContext CreateKarmaContext()
        {
            return m_karmaContext.Value;
        }
    }
}
