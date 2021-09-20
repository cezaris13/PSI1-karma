namespace Karma.Models
{
    public class CharityEvent : IGenericKarmaItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int State { get; set; }

        public CharityEvent (string name, string description, int id, CharityEventState state = CharityEventState.Planning)
        {
            Id = id;
            Name = name;
            Description = description;
            State = (int)state;
        }
    }
}
