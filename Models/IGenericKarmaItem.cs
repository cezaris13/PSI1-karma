namespace Karma.Models
{
    public interface IGenericKarmaItem
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int State { get; set; }
    }
}
