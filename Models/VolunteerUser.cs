using System;
using System.Collections.Generic;

namespace Karma.Models
{
    public class VolunteerUser : IVolunteerUser
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public List<CharityEvent> ParticipatedEvents { get; set; }

        public VolunteerUser(string name, string surname, Guid id)
        {
            Id = id;
            Name = name;
            Surname = surname;
        }
    }
}
