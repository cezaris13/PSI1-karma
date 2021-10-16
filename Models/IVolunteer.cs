using System;
using System.Collections.Generic;

namespace Karma.Models
{
    public interface IVolunteer
    {
        string Name { get; set; }

        string Surname { get; set; }

        Guid Id { get; set; }

        List<CharityEvent> Events { get; set; }
    }
}
