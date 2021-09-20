using System.Threading.Tasks;
using Karma.Models;

namespace Karma.Data
{
    public interface IDataModificator
    {
        Task AddCharityEvent (IGenericKarmaItem newCharityEvent);

        Task RemoveCharityEvent (int charityEventId);

        Task UpdateCharityEvent (IGenericKarmaItem updatedItem);
    }
}
