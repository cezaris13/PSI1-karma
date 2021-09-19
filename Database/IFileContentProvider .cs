using System.Collections.Generic;
using System.Threading.Tasks;
using Karma.Models;

namespace Karma.Database
{
    public interface IFileContentProvider
    {
        Task<IEnumerable<IGenericKarmaItem>> ReadFromFile ();

        Task WriteToFile (IEnumerable<IGenericKarmaItem> listToWrite);
    }
}
