using System;

namespace Karma.Models
{
    public class CharityEvent : IGenericKarmaItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public CharityEventState State { get; set; }

        public CharityEvent(string name, string description, Guid id, CharityEventState state = CharityEventState.Planning)
        {
            Id = id;
            Name = name;
            Description = description;
            State = state;
        }
    }
}
