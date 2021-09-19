using System.Collections.Generic;
using System.Threading.Tasks;
using Karma.Server.Models;

namespace Karma.Server.File
{
    interface IFileContentProvider
    {
        Task<List<Item>> ReadFromFile ();

        Task WriteToFile (List<Item> listToWrite);
    }
}
