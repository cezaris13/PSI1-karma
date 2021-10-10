using System;
using System.Collections.Generic;

namespace Karma.Models
{
    public class CharityEvent : ICharityEvent
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public CharityEventState State { get; set; }

        public List<Volunteer> Volunteers { get; set; }

        public string ManagedByUser { get; set; }

        public CharityEvent(string name, string description, Guid id, string managedByUser, CharityEventState state = CharityEventState.Undefined)
        {
            Id = id;
            Name = name;
            Description = description;
            State = state;
            Volunteers = new List<Volunteer>();
            ManagedByUser = managedByUser;
        }
    }
}
