using System;
using System.Collections.Generic;

namespace Karma.Models
{
    public class PendingVolunteer
    {
        public string Name { get; set; }

        public string Surname { get; set; }
        public string PhoneNumber { get; set; }

        public Guid Id { get; set; }

        public PendingVolunteer(string name, string surname, string phoneNumber, Guid id)
        {
            Id = id;
            Name = name;
            Surname = surname;
            PhoneNumber = phoneNumber;
        }
    }
}
