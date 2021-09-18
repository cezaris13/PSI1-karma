namespace Karma.Server.Models
{
    public enum ItemState
    {
        Started, //charity event
        InProgress, //of collecting money
        Finished, //the event and money collection
        Removed, //the charity event was deleted and is marked for deletion(soft delete).
    }
}
