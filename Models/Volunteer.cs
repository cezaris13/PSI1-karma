using System;
using System.Collections.Generic;

namespace Karma.Models
{
    public class Volunteer : IVolunteer
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public List<CharityEvent> Events { get; set; }

        public Volunteer(string name, string surname, Guid id)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Events = new List<CharityEvent>();
        }
    }
}
