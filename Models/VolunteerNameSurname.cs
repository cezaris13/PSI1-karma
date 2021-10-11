// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Karma.Models
{
    public class VolunteerNameSurname : IVolunteerNameSurname
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
