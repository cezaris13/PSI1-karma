using System;

namespace Karma.Models
{
    public interface IGenericKarmaItem
    {
        Guid Id { get; set; }

        string Name { get; set; }

        string Description { get; set; }

        string ManagerId { get; set; }

        string PhotoPath { get; set; }
    }
}
