// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Karma.Models
{
    public interface ISpecialEquipment
    {
        Guid Id { get; set; }

        string Name { get; set; }

        Volunteer Owner { get; set; }
    }
}
