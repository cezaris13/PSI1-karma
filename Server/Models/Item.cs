namespace Karma.Server.Models
{
    public class Item
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ItemState State { get; set; }

        public int Id { get; set; }

        public Item ()
        {

        }

        public Item (string givenName, string givenDescription, int givenId)
        {
            Name = givenName;
            Description = givenDescription;
            Id = givenId;
            State = ItemState.Ready;
        }
    }
}
