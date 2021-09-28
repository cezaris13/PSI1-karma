using System.Collections.Generic;
using System.Threading.Tasks;
using Karma.Models;

namespace Karma.Database
{
    public interface IFileContentProvider
    {
        Task<IEnumerable<IGenericKarmaItem>> ReadItemFromFileAsync();

        Task<IEnumerable<IVolunteerUser>> ReadVolunteerFromFileAsync();

        Task WriteItemToFileAsync(IEnumerable<IGenericKarmaItem> listToWrite);

        Task WriteVolunteerToFileAsync(IEnumerable<IVolunteerUser> listToWrite);
    }
}
