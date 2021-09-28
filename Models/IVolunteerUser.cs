using System;
using System.Collections.Generic;

namespace Karma.Models
{
    public interface IVolunteerUser
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Surname { get; set; }
        List<CharityEvent> ParticipatedEvents { get; set; }
    }
}
