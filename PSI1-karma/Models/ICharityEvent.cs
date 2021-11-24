// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace Karma.Models
{
    public interface ICharityEvent : IGenericKarmaItem
    {
        CharityEventState State { get; set; }

        List<Volunteer> Volunteers { get; set; }

        int MaxVolunteers { get; set; }
    }
}
