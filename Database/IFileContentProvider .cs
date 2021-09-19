using System.Collections.Generic;
using System.Threading.Tasks;
using Karma.Server.Models;

namespace Karma.Server.Database
{
    public interface IFileContentProvider
    {
        Task<IEnumerable<IGenericKarmaItem>> ReadFromFile ();

        Task WriteToFile (IEnumerable<IGenericKarmaItem> listToWrite);
    }
}
