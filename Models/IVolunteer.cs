using System;
using System.Collections.Generic;

namespace Karma.Models
{
    public interface IVolunteer : IVolunteerNameSurname
    {
        Guid Id { get; set; }
        List<CharityEvent> Events { get; set; }
    }
}
