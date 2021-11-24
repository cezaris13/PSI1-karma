// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Karma.Services
{
    [Serializable]
    public class InvalidAddressException : Exception
    {
        public InvalidAddressException() { }

        public InvalidAddressException(string address)
            : base(string.Format("Invalid address: {0}", address)) { }
    }
}
