using System.Runtime.Serialization;

namespace Karma.Server.Models
{
    [DataContract]
    public class Item
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public ItemState State { get; set; }
        [DataMember]
        public int Id { get; }
        Item (string givenName, string givenDescription, int givenId)
        {
            Name = givenName;
            Description = givenDescription;
            Id = givenId;
            State = ItemState.Ready;
        }
    }
}
