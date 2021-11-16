// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Karma.Models;

namespace Karma.Services
{
    public interface IKarmaContextFactory
    {
        public KarmaContext Create();
    }
}
