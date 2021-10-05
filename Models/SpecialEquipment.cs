// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Karma.Models
{
    public class SpecialEquipment
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Volunteer Owner { get; set; }

        public SpecialEquipment(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
