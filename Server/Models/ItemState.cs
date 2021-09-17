namespace Karma.Server.Models
{
    public enum ItemState
    {
        Ready, //Charity item is publicly seen and ready to be taken
        Reserved, //Someone is interested in this item
        Taken //This item has been collected and is no longer available
    }
}
