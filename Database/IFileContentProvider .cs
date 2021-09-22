using System.Collections.Generic;
using System.Threading.Tasks;
using Karma.Models;

namespace Karma.Database
{
    public interface IFileContentProvider
    {
        Task<IEnumerable<IGenericKarmaItem>> ReadFromFileAsync();

        Task WriteToFileAsync(IEnumerable<IGenericKarmaItem> listToWrite);
    }
}
