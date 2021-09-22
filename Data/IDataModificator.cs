using System;
using System.Threading.Tasks;
using Karma.Models;

namespace Karma.Data
{
    public interface IDataModificator
    {
        Task AddCharityEvent(IGenericKarmaItem newCharityEvent);

        Task RemoveCharityEvent(Guid charityEventId);

        Task UpdateCharityEvent(IGenericKarmaItem updatedItem);
    }
}
