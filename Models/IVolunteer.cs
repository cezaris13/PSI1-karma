using System;
using System.Collections.Generic;

namespace Karma.Models
{
    public interface IVolunteer
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Surname { get; set; }
        List<Guid> ParticipatedEventIds { get; set; }
    }
}
